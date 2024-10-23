using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace WeatherForecast.Core.Helpers;

public class JsonHelper : IJsonHelper
{
    private readonly JsonSerializerOptions _serializeSettings;

    public JsonHelper()
    {
        _serializeSettings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public T? Deserialize<T>(string? json)
    {
        if (json == null)
        {
            throw new InvalidOperationException("Json cannot be null!");
        }
        return JsonSerializer.Deserialize<T>(json, _serializeSettings);
    }
}
