using GitlabMonitor.Extensions;
using GitlabMonitor.Models;

namespace GitlabMonitor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHttpClientFactory _factory;

    public Worker(ILogger<Worker> logger, IHttpClientFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        var client = _factory.CreateGitlabClient();
        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            new Uri("projects/33/merge_requests?state=opened", UriKind.Relative)), token);
        var message = await response.Content.ReadAsStringAsync(token);
        _logger.LogInformation("{Message}", message);

        var merges = message.DeserializeJson<ICollection<MergeRequest>>();
        _logger.LogInformation("{Count}", merges?.Count);
    }
}