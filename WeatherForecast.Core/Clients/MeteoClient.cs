using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp;
using WeatherForecast.Core.Clients.Configuration;
using WeatherForecast.Core.Clients.Providers;
using WeatherForecast.Core.Helpers;
using WeatherForecast.Shared.Exceptions;

namespace WeatherForecast.Core.Clients;

public class MeteoClient : IMeteoClient
{
    private readonly IJsonHelper _jsonHelper;
    private readonly IRestClient _restClient;

    public MeteoClient(IRestClientProvider<MeteoConfiguration> meteoClient, IJsonHelper jsonHelper)
    {
        _jsonHelper = jsonHelper;
        _restClient = meteoClient.Get();
    }

    public async Task<TResponse?> GetWeatherForecastAsync<TResponse>(string path,
        IDictionary<string, string>? queryString = null) where TResponse : class
    {
        var restRequest = new RestRequest(path, Method.Get);
        if (queryString != null)
        {
            foreach (var item in queryString)
            {
                restRequest.AddQueryParameter(item.Key, item.Value);
            }
        }
        
        var restResponse = await _restClient.ExecuteAsync(restRequest);
        if (!restResponse.IsSuccessful)
        {
            throw new ApiException($"Unsuccessful response from {nameof(MeteoClient)} - {nameof(GetWeatherForecastAsync)} method! \nRest request:\n{restRequest} \n Rest response:\n{restResponse}", 
                "Cannot get weather forecast!");
        }

        return _jsonHelper.Deserialize<TResponse>(restResponse.Content);
    }
}