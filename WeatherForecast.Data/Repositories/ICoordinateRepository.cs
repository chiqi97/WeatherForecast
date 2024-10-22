using WeatherForecast.Data.Entities;

namespace WeatherForecast.Data.Repositories;

public interface ICoordinateRepository
{
    IQueryable<Coordinate> GetQuery();
    Task<int> AddOrUpdateAsync(double latitude, double longitude, Entities.WeatherForecast weatherForecastEntity);
}