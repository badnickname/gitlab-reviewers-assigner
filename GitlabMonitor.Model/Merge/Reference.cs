namespace GitlabMonitor.Model.Merge;

/// <summary>
///     Reference из Merge Request
/// </summary>
[Serializable]
public sealed class Reference
{
    public string Full { get; set; }
}