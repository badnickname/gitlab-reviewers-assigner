using System.Text;
using Dapper;
using GitlabMonitor.Extensions;
using GitlabMonitor.Model;
using GitlabMonitor.Model.Merge;
using GitlabMonitor.Model.Statistic;
using Microsoft.EntityFrameworkCore;

namespace GitlabMonitor.Infrastructure;

public sealed class ContextImplementation : IContext
{
    private readonly IHttpClientFactory _factory;
    private readonly ApplicationContext _context;

    public ContextImplementation(IHttpClientFactory factory, ApplicationContext context)
    {
        _factory = factory;
        _context = context;
    }

    public async Task CreateUsers(ICollection<int> userIds, CancellationToken token)
    {
        var connection = _context.Database.GetDbConnection();
        
        foreach (var userId in userIds)
        {
            var count = await connection.QuerySingleAsync<int>("SELECT COUNT(1) FROM Reviewers WHERE UserId = @UserId",
                new { UserId = userId });
            if (count > 0) continue;
            _context.Reviewers.Add(new Reviewer
            {
                UserId = userId
            });
            await _context.SaveChangesAsync(token);
        }
    }

    public async Task<int> GetBotUserIdAsync(CancellationToken token)
    {
        var client = _factory.CreateGitlabClient();
        var response = await client.GetAsync(new Uri("user", UriKind.Relative), token);
        var content = await response.Content.ReadAsStringAsync(token);
        var user = content.DeserializeJson<User>();
        return user.Id;
    }

    public async Task<ICollection<MergeRequest>> GetMergeRequestsFromProjectsAsync(ICollection<int> projectIds, CancellationToken token)
    {
        var client = _factory.CreateGitlabClient();
        IEnumerable<MergeRequest> merges = Array.Empty<MergeRequest>();
        foreach (var projectId in projectIds)
        {
            var response = await client.GetAsync(new Uri($"projects/{projectId}/merge_requests?state=opened", UriKind.Relative), token);
            var content = await response.Content.ReadAsStringAsync(token);
            merges = merges.Concat(content.DeserializeJson<ICollection<MergeRequest>>());
        }

        return merges.ToList();
    }

    public async Task<ICollection<(int UserId, int Count)>> GetAssignedMergeRequestsCountAsync(CancellationToken token)
    {
        var connection = _context.Database.GetDbConnection();
        var result = (await connection.QueryAsync(
                @"SELECT 
                    R.UserId as UserId, 
                    (SELECT COUNT(1) FROM AssignedMergeRequests AMR WHERE AMR.ReviewerId = R.Id) as Count
                    FROM Reviewers R", token))
            .Select(x => ((int) x.UserId, (int) x.Count))
            .ToList();
        return result;
    }

    public async Task<ICollection<AssignedMergeRequest>> GetLastAssignedMergeRequests(CancellationToken token)
    {
        var result = await _context.AssignedMergeRequests
            .OrderByDescending(x => x.Id)
            .Include(x => x.Reviewer)
            .AsNoTracking()
            .Take(20)
            .ToListAsync(token);

        return result;
    }

    public async Task AssignToMergeRequestAsync(int projectId, int mergeRequestId, string title, string reference, int userId, CancellationToken token)
    {
        var connection = _context.Database.GetDbConnection();
        var reviewerId = await connection.QueryFirstAsync<int>("SELECT Id FROM Reviewers WHERE UserId = @UserId",
            new { UserId = userId });

        _context.AssignedMergeRequests.Add(new AssignedMergeRequest
        {
            ReviewerId = reviewerId,
            Title = title,
            References = reference,
            ProjectId = projectId,
            MergeId = mergeRequestId
        });

        await _context.SaveChangesAsync(token);
        
        var client = _factory.CreateGitlabClient();
        var payload = new
        {
            ReviewerIds = new []
            {
                userId
            }
        };
        var request = new StringContent(payload.SerializeJson(), Encoding.UTF8, "application/json");
        await client.PutAsync(new Uri($"projects/{projectId}/merge_requests/{mergeRequestId}", UriKind.Relative), request, token);
    }
}