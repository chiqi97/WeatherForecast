using WeatherForecast.Core.Models.GeoLocation;

namespace WeatherForecast.Core.Services;

public interface IGeoLocationService
{
    Task<IList<GeoLocationDto>> GetPreviouslyUsed(int pageNumber = 1, int pageSize = 10);
}