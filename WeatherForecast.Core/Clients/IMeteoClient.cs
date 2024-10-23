using WeatherForecast.Core.Models.WeatherForecast;

namespace WeatherForecast.Core.Clients;

public interface IMeteoClient
{
    Task<Models.Clients.MeteoClient.MeteoWeatherForecast?> GetWeatherForecastAsync(AddWeatherForecast weatherForecast);
}