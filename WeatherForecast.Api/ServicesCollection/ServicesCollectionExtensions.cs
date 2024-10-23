using Microsoft.EntityFrameworkCore;
using WeatherForecast.Core.Clients;
using WeatherForecast.Core.Clients.Configuration;
using WeatherForecast.Core.Clients.Providers;
using WeatherForecast.Core.Helpers;
using WeatherForecast.Core.Services;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Repositories;
using WeatherForecastAPI.Middlewares;

namespace WeatherForecastAPI.ServicesCollection;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddWeatherForecastServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ExceptionHandlingMiddleware>();
        services.AddSingleton<IRestClientProvider<MeteoConfiguration>, MeteoRestClientProvider>();
        services.AddScoped<IWeatherRepository, WeatherRepository>();
        services.AddScoped<ICoordinateRepository, CoordinateRepository>();
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        services.AddScoped<IGeoLocationService, GeoLocationService>();
        services.AddScoped<IMeteoClient, MeteoClient>();
        services.AddScoped<IJsonHelper, JsonHelper>();
        services.Configure<MeteoConfiguration>(configuration.GetSection("MeteoConfiguration"));
        services.AddEntityFrameworkSqlite().AddDbContext<WeatherForecastDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DbConnectionString")));

        return services;
    }
}