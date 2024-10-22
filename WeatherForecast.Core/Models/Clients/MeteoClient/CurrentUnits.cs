using System.Text.Json.Serialization;

namespace WeatherForecast.Core.Models.Clients.MeteoClient;

public class CurrentUnits
{
    public string? Time { get; set; } 

    public string? Interval { get; set; } 

    [JsonPropertyName("temperature_2m")]
    public string? Temperature2m { get; set; } 
}