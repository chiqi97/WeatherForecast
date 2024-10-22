namespace WeatherForecast.Core.Clients;

public interface IMeteoClient
{
    Task<TResponse?> GetWeatherForecastAsync<TResponse>(string path,
        IDictionary<string, string>? queryString = null) where TResponse : class;
}