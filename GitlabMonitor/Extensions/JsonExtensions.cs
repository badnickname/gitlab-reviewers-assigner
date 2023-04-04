using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GitlabMonitor.Extensions;

public static class JsonExtensions
{
    public static T DeserializeJson<T>(this string json)
    {
        var result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        });

        return result!;
    }
    
    public static string SerializeJson<T>(this T obj) {
        var result = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        });

        return result;
    }
}