namespace WeatherForecast.Core.Helpers;

public interface IJsonHelper
{
    T? Deserialize<T>(string? json);
}