using WeatherForecast.Core.Models.Coordinate;

namespace WeatherForecast.Core.Models.WeatherForecast;

public class WeatherForecastDto
{
    public int Id { get; set; }
    public double Temperature { get; set; }
    public double Interval { get; set; }
    public DateTime Time { get; set; }
    public CoordinateWeatherDto Coordinate { get; set; }
}