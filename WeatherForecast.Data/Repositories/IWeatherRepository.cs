using WeatherForecast.Data.Entities;

namespace WeatherForecast.Data.Repositories;

public interface IWeatherRepository
{
    Task<int> AddOrUpdateAsync(decimal latitude, decimal longitude, Entities.WeatherForecast weatherForecastEntity);
    Task<Entities.WeatherForecast> GetById(int id);
    Task<int> DeleteAsync(int id);
}