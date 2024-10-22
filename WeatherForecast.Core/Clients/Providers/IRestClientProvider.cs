using RestSharp;

namespace WeatherForecast.Core.Clients.Providers;

public interface IRestClientProvider<T> where T : class
{
    IRestClient Get();
}