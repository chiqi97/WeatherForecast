using WeatherForecast.Data.Entities;

namespace WeatherForecast.Data.Repositories;

public interface IWeatherRepository
{
    Task<Entities.WeatherForecast> GetById(int id);
    Task<int> DeleteAsync(int id);
}