namespace GitlabMonitor.Models;

[Serializable]
public sealed class MergeRequest
{
    public int Id { get; set; }
    
    public string Title { get; set; }
}