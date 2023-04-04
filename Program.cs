using GitlabMonitor;
using GitlabMonitor.Extensions;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.AddConfiguration(config.GetSection("Logging"));
    })
    .ConfigureServices(services =>
    {
        services.AddGitlabClient(config.GetValue<string>("Gitlab:Url"), config.GetValue<string>("Gitlab:Token"));
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();