namespace GitlabMonitor.Extensions;

public static class HttpClientExtensions
{
    private const string Gitlab = "Gitlab";

    public static HttpClient CreateGitlabClient(this IHttpClientFactory factory)
    {
        return factory.CreateClient(Gitlab);
    }

    public static void AddGitlabClient(this IServiceCollection services, string url, string token)
    {
        services.AddHttpClient(Gitlab, options =>
        {
            options.BaseAddress = new Uri($"{url}/api/v4/", UriKind.Absolute);
            options.DefaultRequestHeaders.Add("PRIVATE-TOKEN", token);
        });
    }
}