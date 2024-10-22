namespace WeatherForecast.Data.Entities;

public record WeatherForecast : Entity
{
    public double Temperature { get; set; }
    public double Interval { get; set; }
    public DateTime Time { get; set; }
    public Coordinate Coordinate { get; set; }
    public int CoordinateId { get; set; }
}