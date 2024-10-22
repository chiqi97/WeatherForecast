namespace WeatherForecast.Core.Models.GeoLocation;

public class GeoLocationDto
{
    public GeoLocationCoordinateDto Coordinate { get; set; }
    public GeoLocationWeatherForecastDto WeatherForecast { get; set; }
}