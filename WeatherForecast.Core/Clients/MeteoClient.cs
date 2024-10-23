using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using RestSharp;
using WeatherForecast.Core.Clients.Configuration;
using WeatherForecast.Core.Clients.Providers;
using WeatherForecast.Core.Helpers;
using WeatherForecast.Core.Models.WeatherForecast;
using WeatherForecast.Shared.Exceptions;

namespace WeatherForecast.Core.Clients;

public class MeteoClient : IMeteoClient
{
    private readonly IJsonHelper _jsonHelper;
    private readonly IRestClient _restClient;
    private readonly MeteoConfiguration _configuration;

    public MeteoClient(IRestClientProvider<MeteoConfiguration> meteoClient, IJsonHelper jsonHelper, IOptions<MeteoConfiguration> configuration)
    {
        _jsonHelper = jsonHelper;
        _restClient = meteoClient.Get();
        _configuration = configuration.Value;
    }

    public async Task<Models.Clients.MeteoClient.MeteoWeatherForecast?> GetWeatherForecastAsync(AddWeatherForecast addWeatherForecast)
    {
        var restRequest = new RestRequest(_configuration.GetWeatherForecastPath, Method.Get);
        restRequest.AddQueryParameter("latitude", addWeatherForecast.Latitude.ToString());
        restRequest.AddQueryParameter("longitude", addWeatherForecast.Longitude.ToString());
        restRequest.AddQueryParameter("current", addWeatherForecast.Current.ToString());
        
        var restResponse = await _restClient.ExecuteAsync(restRequest);
        if (!restResponse.IsSuccessful)
        {
            throw new ApiException($"Unsuccessful response from {nameof(MeteoClient)} - {nameof(GetWeatherForecastAsync)} method! \nRest request:\n{restRequest} \n Rest response:\n{restResponse}", 
                "Cannot get weather forecast!");
        }

        return _jsonHelper.Deserialize<Models.Clients.MeteoClient.MeteoWeatherForecast>(restResponse.Content);
    }
}