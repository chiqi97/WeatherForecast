using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Core.Models.WeatherForecast;

public record AddWeatherForecast
{
    [Range(-180d, 180d, ErrorMessage = "Longitude must be between -180 and 180.")]
    public decimal Longitude  { get; set; }
    [Range(-90d, 90d, ErrorMessage = "Latitude must be between -90 and 90.")]
    public decimal Latitude { get; set; }
    public CurrentTemperature Current { get; set; }
}