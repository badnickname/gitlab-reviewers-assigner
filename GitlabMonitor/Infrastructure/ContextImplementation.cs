using GitlabMonitor.Model;
using GitlabMonitor.Model.Merge;
using GitlabMonitor.Model.Statistic;

namespace GitlabMonitor.Infrastructure;

public sealed class ContextImplementation : IContext
{
    private readonly IHttpClientFactory _factory;

    public ContextImplementation(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public Task CreateUsers(ICollection<int> userIds, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetBotUserIdAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<MergeRequest>> GetMergeRequestsFromProjectsAsync(ICollection<int> projectIds, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<(int UserId, int Count)>> GetAssignedMergeRequestsCountAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<AssignedMergeRequest>> GetLastAssignedMergeRequests(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task AssignToMergeRequestAsync(int projectId, int mergeRequestId, int userId, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}