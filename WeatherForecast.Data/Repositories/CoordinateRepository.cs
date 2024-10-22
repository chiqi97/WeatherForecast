using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Entities;

namespace WeatherForecast.Data.Repositories;

public class CoordinateRepository : BaseRepository<Coordinate>, ICoordinateRepository
{
    public CoordinateRepository(WeatherForecastDbContext context) : base(context)
    {
    }

    public IQueryable<Coordinate> GetQuery() => GetEntitiesQuery();
    
}