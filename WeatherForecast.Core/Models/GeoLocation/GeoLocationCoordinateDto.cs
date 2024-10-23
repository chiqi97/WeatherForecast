namespace WeatherForecast.Core.Models.GeoLocation;

public record GeoLocationCoordinateDto
{
    public int Id { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime LastRequestTime { get; set; }
}