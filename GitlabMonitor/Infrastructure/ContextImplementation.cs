using Dapper;
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

    public Task<int> GetBotUserIdAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<MergeRequest>> GetMergeRequestsFromProjectsAsync(ICollection<int> projectIds, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<(int UserId, int Count)>> GetAssignedMergeRequestsCountAsync(CancellationToken token)
    {
        var connection = _context.Database.GetDbConnection();
        var result = (await connection.QueryAsync(
                @"SELECT R.UserId as UserId, COUNT(AMR.Id) as Count 
        FROM AssignedMergeRequests AMR, Reviewers R 
        WHERE R.Id = AMR.ReviewerId
        GROUP BY ReviewerId", token))
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
        var reviewerId = await connection.QueryFirstAsync<int>("SELECT ReviewerId FROM Reviewers WHERE UserId = @UserId",
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
        
        throw new NotImplementedException();
    }
}