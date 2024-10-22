namespace WeatherForecast.Core.Helpers;

public interface IJsonHelper
{
    string Serialize(object obj);
    T? Deserialize<T>(string? json);
}