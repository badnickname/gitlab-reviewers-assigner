namespace GitlabMonitor.Model.Merge;

/// <summary>
///     Открытый Merge Request
/// </summary>
[Serializable]
public sealed class MergeRequest
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public string Title { get; set; }

    public bool Draft { get; set; }

    public User Author { get; set; }

    public User Assignee { get; set; }

    public ICollection<User> Reviewers { get; set; }

    public Reference References { get; set; }
}