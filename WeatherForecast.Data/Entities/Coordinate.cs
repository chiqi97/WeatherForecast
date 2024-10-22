namespace WeatherForecast.Data.Entities;

public record Coordinate : Entity
{
    public decimal Longitude  { get; set; }
    public decimal Latitude  { get; set; }
    public ICollection<WeatherForecast> WeatherForecasts { get; set; }
}