using Microsoft.Extensions.Options;
using RestSharp;
using WeatherForecast.Core.Clients.Configuration;

namespace WeatherForecast.Core.Clients.Providers;

public class MeteoRestClientProvider : IRestClientProvider<MeteoConfiguration>
{
    private readonly IRestClient _restClient;

    public MeteoRestClientProvider(IOptions<MeteoConfiguration> configuration)
    {
        _restClient = new RestClient(new Uri(configuration.Value.Host));
    }
    public IRestClient Get()
    {
        return _restClient;
    }
}