using WeatherForecast.Core.Models.WeatherForecast;

namespace WeatherForecast.Core.Services;

public interface IWeatherForecastService
{
    Task<int> AddWeatherForecastAndCoordinatesAsync(AddWeatherForecast addWeatherForecast);
    Task<WeatherForecastDto> GetByIdAsync(int id);
    Task<int> DeleteAsync(int id);
}