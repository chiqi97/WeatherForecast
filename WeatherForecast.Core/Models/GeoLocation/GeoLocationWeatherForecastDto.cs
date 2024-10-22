namespace WeatherForecast.Core.Models.GeoLocation;

public class GeoLocationWeatherForecastDto
{
    public int Id { get; set; }
    public double Temperature { get; set; }
    public double Interval { get; set; }
    public DateTime Time { get; set; }
}