using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Entities;

namespace WeatherForecast.Data.Repositories;

public class WeatherRepository : BaseRepository<Entities.WeatherForecast>, IWeatherRepository
{
    public WeatherRepository(WeatherForecastDbContext context) : base(context)
    {
    }

    public async Task<int> AddOrUpdateAsync(decimal latitude, decimal longitude, Entities.WeatherForecast weatherForecastEntity)
    {
        var existingCoordinate = await _context.Coordinates.FirstOrDefaultAsync(c => c.Longitude == longitude && c.Latitude == latitude);
        if (existingCoordinate == null)
        {
            weatherForecastEntity.Coordinate = new Coordinate() {Latitude = latitude, Longitude = longitude, LastRequestTime = TimeProvider.System.GetUtcNow().DateTime};
            return await AddEntityAsync(weatherForecastEntity);
        }

        weatherForecastEntity.CoordinateId = existingCoordinate.Id;
        existingCoordinate.LastRequestTime = TimeProvider.System.GetUtcNow().DateTime;
        return await AddEntityAsync(weatherForecastEntity);
    }

    public async Task<Entities.WeatherForecast> GetById(int id)
    {
        var weatherForecast = await _context.WeatherForecasts
            .Include(x => x.Coordinate)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        ThrowIfNotExists(weatherForecast, id);

        return weatherForecast!;
    }

    public async Task<int> DeleteAsync(int id) => await DeleteEntityAsync(id);
}