using System.Text.Json.Serialization;

namespace WeatherForecast.Core.Models.Clients.MeteoClient;

public record CurrentWeather
{
    public DateTime? Time { get; set; } 

    public int Interval { get; set; } 

    [JsonPropertyName("temperature_2m")]
    public double Temperature2M { get; set; } 
}