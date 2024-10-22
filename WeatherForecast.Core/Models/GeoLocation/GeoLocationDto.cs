namespace WeatherForecast.Core.Models.GeoLocation;

public record GeoLocationDto
{
    public GeoLocationCoordinateDto Coordinate { get; set; }
    public GeoLocationWeatherForecastDto WeatherForecast { get; set; }
}