using GitlabMonitor.Infrastructure;
using GitlabMonitor.Model;
using GitlabMonitor.Model.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;

namespace GitlabMonitor;

public sealed class AssignJob : IJob
{
    private readonly IContext _context;
    private readonly ApplicationContext _application;
    
    private readonly ILogger<AssignJob> _logger;
    private readonly IOptionsSnapshot<Projects> _projects;
    private readonly IOptionsSnapshot<UserIds> _userIds;

    public AssignJob(ILogger<AssignJob> logger, IOptionsSnapshot<Projects> projects, IContext context,
        IOptionsSnapshot<UserIds> userIds, ApplicationContext application)
    {
        _logger = logger;
        _projects = projects;
        _context = context;
        _userIds = userIds;
        _application = application;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Start job...");
        await using var transaction = await _application.Database.BeginTransactionAsync();
        try
        {
            var bot = new MergeRequestBot(_context, _userIds.Value, _projects.Value);
            await bot.AssignFolksAsync(context.CancellationToken);
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("{Error}", e);
            await transaction.RollbackAsync();
        }
    }
}