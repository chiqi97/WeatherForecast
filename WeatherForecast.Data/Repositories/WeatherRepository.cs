using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Entities;

namespace WeatherForecast.Data.Repositories;

public class WeatherRepository : BaseRepository<Entities.WeatherForecast>, IWeatherRepository
{
    public WeatherRepository(WeatherForecastDbContext context) : base(context)
    {
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