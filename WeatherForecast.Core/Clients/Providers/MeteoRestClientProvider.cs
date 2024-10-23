using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using WeatherForecast.Core.Clients.Configuration;

namespace WeatherForecast.Core.Clients.Providers;

public class MeteoRestClientProvider : IRestClientProvider<MeteoConfiguration>
{
    private readonly IRestClient _restClient;


    public MeteoRestClientProvider(IOptions<MeteoConfiguration> configuration)
    {
        _restClient = new RestClient(new Uri(configuration.Value.Host), configureSerialization: s =>
        {
            s.UseSystemTextJson(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
        });
    }
    public IRestClient Get()
    {
        return _restClient;
    }
}