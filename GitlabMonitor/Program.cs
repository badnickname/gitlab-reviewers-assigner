using GitlabMonitor;
using GitlabMonitor.Extensions;
using GitlabMonitor.Infrastructure;
using GitlabMonitor.Model;
using GitlabMonitor.Model.Config;
using Quartz;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder => { builder.AddConfiguration(config.GetSection("Logging")); })
    .ConfigureServices(services =>
    {
        services.Configure<Projects>(config.GetSection("Gitlab:Projects"));
        services.Configure<UserIds>(config.GetSection("Gitlab:UserIds"));
        services.AddScoped<IContext, ContextImplementation>();
        services.AddGitlabClient(config.GetValue<string>("Gitlab:Url"), config.GetValue<string>("Gitlab:Token"));
        services.AddSqlite<ApplicationContext>(config.GetValue<string>("Sqlite:Connection"));
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("AssignJob");
            q.AddJob<AssignJob>(options => options.WithIdentity(jobKey));
            q.AddTrigger(options => options
                .ForJob(jobKey)
                .WithCronSchedule("0 0-59 9-19 ? * 1-5"));
        });
        services.AddQuartzHostedService();
    })
    .Build()
    .Run();