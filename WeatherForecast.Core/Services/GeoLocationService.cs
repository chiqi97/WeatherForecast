using Microsoft.EntityFrameworkCore;
using WeatherForecast.Core.Models.GeoLocation;
using WeatherForecast.Data.Entities;
using WeatherForecast.Data.Repositories;

namespace WeatherForecast.Core.Services;

public class GeoLocationService : IGeoLocationService
{
    private readonly ICoordinateRepository _coordinateRepository;

    public GeoLocationService(ICoordinateRepository coordinateRepository)
    {
        _coordinateRepository = coordinateRepository;
    }

    public async Task<IList<GeoLocationDto>> GetPreviouslyUsed(int pageNumber = 1, int pageSize = 10)
    {
        var coordinates = await _coordinateRepository.GetQuery()
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new Coordinate()
            {
                Id = c.Id,
                Longitude = c.Longitude,
                Latitude = c.Latitude,
                WeatherForecasts  = c.WeatherForecasts
                    .OrderByDescending(w => w.Time)
                    .ToList() 
            })
            .ToListAsync();
        
        
        var geoLocationDtos = coordinates.Select(x => new GeoLocationDto()
        {
            Coordinate = new GeoLocationCoordinateDto()
            {
                Id = x.Id,
                Latitude = x.Latitude, 
                Longitude = x.Longitude, 
            },
            WeatherForecast = x.WeatherForecasts.FirstOrDefault() != null ? new GeoLocationWeatherForecastDto()
            {
                Id = x.WeatherForecasts.FirstOrDefault().Id,
                Temperature = x.WeatherForecasts.FirstOrDefault().Temperature,
                Time = x.WeatherForecasts.FirstOrDefault().Time,
                Interval = x.WeatherForecasts.FirstOrDefault().Interval
            } : null
                
        }).ToList();
        
        
        return geoLocationDtos;

    }
}