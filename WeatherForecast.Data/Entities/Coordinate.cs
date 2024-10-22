namespace WeatherForecast.Data.Entities;

public record Coordinate : Entity
{
    public double Longitude  { get; set; }
    public double Latitude  { get; set; }
    public ICollection<WeatherForecast> WeatherForecasts { get; set; }
}