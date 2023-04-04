namespace GitlabMonitor.Model.Models;

[Serializable]
public sealed class User
{
    public int Id { get; set; }
    
    public string Username { get; set; }
}