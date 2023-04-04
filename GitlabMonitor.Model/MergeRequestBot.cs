using GitlabMonitor.Model.Config;

namespace GitlabMonitor.Model;

public sealed class MergeRequestBot
{
    private readonly IContext _context;
    private readonly Usernames _usernames;

    public MergeRequestBot(IContext context, Usernames usernames)
    {
        _context = context;
        _usernames = usernames;
    }

    public async Task AssignFolksAsync(CancellationToken token)
    {
        var users = await _context.GetUsersByUsernamesAsync(_usernames, token);
    }
}