using WeatherForecast.Data.Entities;

namespace WeatherForecast.Data.Repositories;

public interface ICoordinateRepository
{
    IQueryable<Coordinate> GetQuery();
}