using Microsoft.Extensions.Options;
using WeatherForecast.Core.Clients;
using WeatherForecast.Core.Clients.Configuration;
using WeatherForecast.Core.Models.Coordinate;
using WeatherForecast.Core.Models.WeatherForecast;
using WeatherForecast.Data.Repositories;

namespace WeatherForecast.Core.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly IMeteoClient _meteoClient;
    private readonly MeteoConfiguration _configuration;
    private readonly ICoordinateRepository _coordinateRepository;

    public WeatherForecastService(IWeatherRepository weatherRepository, ICoordinateRepository coordinateRepository, IMeteoClient meteoClient, IOptions<MeteoConfiguration> configuration)
    {
        _weatherRepository = weatherRepository;
        _meteoClient = meteoClient;
        _configuration = configuration.Value;
        _coordinateRepository = coordinateRepository;
    }

    public async Task<int> AddWeatherForecastAndCoordinatesAsync(AddWeatherForecast addWeatherForecast)
    {
        var queryString = new Dictionary<string, string>()
        {
            {"latitude", addWeatherForecast.Latitude.ToString()},
            {"longitude", addWeatherForecast.Longitude.ToString()},
            {"current", addWeatherForecast.Current.ToString()}
        };
        
        var weatherForecast = await 
            _meteoClient.GetWeatherForecastAsync<Models.Clients.MeteoClient.WeatherForecast>(_configuration.GetWeatherForecastPath, queryString);
        var weatherEntity = new Data.Entities.WeatherForecast()
        {
            Time = weatherForecast?.Current?.Time ?? TimeProvider.System.GetUtcNow().UtcDateTime,
            Temperature = weatherForecast.Current.Temperature2M,
            Interval = weatherForecast.Current.Interval

        };
     
        return  await _coordinateRepository.AddOrUpdateAsync(addWeatherForecast.Latitude, addWeatherForecast.Longitude, weatherEntity);
    }

    public async Task<WeatherForecastDto> GetByIdAsync(int id)
    {
        var entity = await _weatherRepository.GetById(id);
        return new WeatherForecastDto()
        {
            Interval = entity.Interval,
            Temperature = entity.Temperature,
            Id = entity.Id,
            Time = entity.Time,
            Coordinate = new CoordinateWeatherDto(){Longitude = entity.Coordinate.Longitude, Latitude = entity.Coordinate.Latitude, Id = entity.Coordinate.Id}
        };
    }
    
    public async Task<int> DeleteAsync(int id)
    {
        var result = await _weatherRepository.DeleteAsync(id);
        return result;
    }
     
}