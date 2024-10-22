namespace WeatherForecast.Core.Models.Coordinate;

public record CoordinateWeatherDto
{
    public int Id { get; set; }
    public decimal Longitude  { get; set; }
    public decimal Latitude  { get; set; }
}