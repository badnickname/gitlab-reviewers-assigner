namespace GitlabMonitor.Model.Statistic;

/// <summary>
///     Ревьюер
/// </summary>
public sealed class Reviewer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public ICollection<AssignedMergeRequest> AssignedMergeRequests { get; set; }
}