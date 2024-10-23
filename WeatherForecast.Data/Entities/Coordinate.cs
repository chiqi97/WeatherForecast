namespace WeatherForecast.Data.Entities;

public class Coordinate : Entity
{
    public decimal Longitude  { get; set; }
    public decimal Latitude  { get; set; }
    public DateTime LastRequestTime { get; set; }
    public ICollection<WeatherForecast> WeatherForecasts { get; set; }
}