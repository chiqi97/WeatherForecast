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

    public async Task<int> AddOrUpdateAsync(double latitude, double longitude, Entities.WeatherForecast weatherForecastEntity)
    {
       var existingCoordinate = await _context.Coordinates.Include(x=> x.WeatherForecasts).FirstOrDefaultAsync(c => c.Longitude == longitude && c.Latitude == latitude);
       if (existingCoordinate != null)
       {
           existingCoordinate.WeatherForecasts.Add(weatherForecastEntity);
           await _context.SaveChangesAsync();
           return existingCoordinate.Id;
       }

       var coordinatEntity = new Coordinate()
       {
           Longitude = longitude,
           Latitude = latitude,
           WeatherForecasts = new List<Entities.WeatherForecast>()
           {
               weatherForecastEntity
           }
       };
       return await AddEntityAsync(coordinatEntity);

    }
}