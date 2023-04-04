namespace GitlabMonitor.Model.Statistic;

/// <summary>
///     Обработанный Merge Request
/// </summary>
public sealed class AssignedMergeRequest
{
    public int Id { get; set; }

    public int ReviewerId { get; set; }

    public Reviewer Reviewer { get; set; }

    public string Title { get; set; }

    public string References { get; set; }
}