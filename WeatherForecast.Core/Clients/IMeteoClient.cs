using WeatherForecast.Core.Models.WeatherForecast;

namespace WeatherForecast.Core.Clients;

public interface IMeteoClient
{
    Task<Models.Clients.MeteoClient.WeatherForecast?> GetWeatherForecastAsync(AddWeatherForecast weatherForecast);
}