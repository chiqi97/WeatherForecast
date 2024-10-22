using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Core.Models.WeatherForecast;

public class AddWeatherForecast
{
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
    public double Longitude  { get; set; }
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
    public double Latitude { get; set; }
    public CurrentTemperature Current { get; set; }
}