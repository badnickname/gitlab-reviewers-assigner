using GitlabMonitor.Model.Config;

namespace GitlabMonitor.Model;

public sealed class MergeRequestBot
{
    private readonly IContext _context;
    private readonly Projects _projects;
    private readonly UserIds _userIds;

    public MergeRequestBot(IContext context, UserIds userIds, Projects projects)
    {
        _context = context;
        _userIds = userIds;
        _projects = projects;
    }

    public async Task AssignFolksAsync(CancellationToken token)
    {
        var botId = await _context.GetBotUserIdAsync(token);

        await _context.CreateUsers(_userIds, token);

        var merges = await _context.GetMergeRequestsFromProjectsAsync(_projects, token);

        var botMergeRequests = merges.Where(x => x.Reviewers.Any(x => x.Id == botId) || x.Assignee?.Id == botId);

        var mergeRequestsWithReviewer = merges.Where(x => x.Reviewers.Any(x => x.Id != botId)).ToList();

        foreach (var mergeRequest in botMergeRequests)
        {
            var projectId = mergeRequest.ProjectId;
            var mergeId = mergeRequest.Id;

            var isAssigned = false;

            foreach (var assignedMerge in mergeRequestsWithReviewer)
                if (assignedMerge.References?.Full == mergeRequest.References?.Full ||
                    CalculateSimilarity(assignedMerge.Title, mergeRequest.Title) > 0.8)
                {
                    var userId = assignedMerge.Reviewers.First().Id;
                    if (!_userIds.Contains(userId))
                        break;
                    await _context.AssignToMergeRequestAsync(projectId, mergeId, mergeRequest.Title, mergeRequest.References?.Full, userId, token);
                    isAssigned = true;
                    break;
                }

            if (isAssigned)
                continue;
            
            var lastMerges = (await _context.GetLastAssignedMergeRequests(token))
                .Where(x => x.ProjectId != projectId || x.MergeId != mergeId);
            
            foreach (var assignedMerge in lastMerges)
            {
                if (assignedMerge.References == mergeRequest.References?.Full ||
                    CalculateSimilarity(assignedMerge.Title, mergeRequest.Title) > 0.5)
                {
                    var userId = assignedMerge.Reviewer.UserId;
                    if (!_userIds.Contains(userId))
                        break;
                    await _context.AssignToMergeRequestAsync(projectId, mergeId, mergeRequest.Title, mergeRequest.References?.Full, userId, token);
                    isAssigned = true;
                    break;
                }
            }
            
            if (isAssigned)
                continue;

            var usersLoad = await _context.GetAssignedMergeRequestsCountAsync(token);

            var suggestedUserId = usersLoad
                .Where(x => _userIds.Contains(x.UserId))
                .MinBy(x => x.Count).UserId;

            await _context.AssignToMergeRequestAsync(projectId, mergeId, mergeRequest.Title, mergeRequest.References?.Full, suggestedUserId, token);
        }
    }

    /// <summary>
    ///     Calculate percentage similarity of two strings
    ///     <param name="source">Source String to Compare with</param>
    ///     <param name="target">Targeted String to Compare</param>
    ///     <returns>Return Similarity between two strings from 0 to 1.0</returns>
    /// </summary>
    private double CalculateSimilarity(string source, string target)
    {
        if (source == null || target == null) return 0.0;
        if (source.Length == 0 || target.Length == 0) return 0.0;
        if (source == target) return 1.0;

        var stepsToSame = ComputeLevenshteinDistance(source, target);
        return 1.0 - stepsToSame / (double) Math.Max(source.Length, target.Length);
    }

    /// <summary>
    ///     Returns the number of steps required to transform the source string
    ///     into the target string.
    /// </summary>
    private int ComputeLevenshteinDistance(string source, string target)
    {
        if (source == null || target == null) return 0;
        if (source.Length == 0 || target.Length == 0) return 0;
        if (source == target) return source.Length;

        var sourceWordCount = source.Length;
        var targetWordCount = target.Length;

        // Step 1
        if (sourceWordCount == 0)
            return targetWordCount;

        if (targetWordCount == 0)
            return sourceWordCount;

        var distance = new int[sourceWordCount + 1, targetWordCount + 1];

        // Step 2
        for (var i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
        for (var j = 0; j <= targetWordCount; distance[0, j] = j++) ;

        for (var i = 1; i <= sourceWordCount; i++)
        for (var j = 1; j <= targetWordCount; j++)
        {
            // Step 3
            var cost = target[j - 1] == source[i - 1] ? 0 : 1;

            // Step 4
            distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                distance[i - 1, j - 1] + cost);
        }

        return distance[sourceWordCount, targetWordCount];
    }
}