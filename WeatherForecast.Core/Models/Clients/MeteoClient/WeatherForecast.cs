using System.Text.Json.Serialization;

namespace WeatherForecast.Core.Models.Clients.MeteoClient;

public class WeatherForecast
{
    public string? Timezone { get; set; } 

    [JsonPropertyName("timezone_abbreviation")]
    public string? TimezoneAbbreviation { get; set; } 

    public double Elevation { get; set; } 
    
    [JsonPropertyName("current_units")]
    public CurrentUnits? CurrentUnits { get; set; } 
    public CurrentWeather? Current { get; set; } 
}