
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.Core.Clients;
using WeatherForecast.Core.Clients.Configuration;
using WeatherForecast.Core.Clients.Providers;
using WeatherForecast.Core.Helpers;
using WeatherForecast.Core.Services;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Entities;
using WeatherForecast.Data.Repositories;
using WeatherForecastAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<IRestClientProvider<MeteoConfiguration>, MeteoRestClientProvider>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<ICoordinateRepository, CoordinateRepository>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();
builder.Services.AddScoped<IMeteoClient, MeteoClient>();
builder.Services.AddScoped<IJsonHelper, JsonHelper>();
builder.Services.Configure<MeteoConfiguration>(builder.Configuration.GetSection("MeteoConfiguration"));
builder.Services.AddEntityFrameworkSqlite().AddDbContext<WeatherForecastDbContext>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//using var scope = app.Services.CreateScope();
//var db = scope.ServiceProvider.GetRequiredService<WeatherForecastDbContext>();
//db.Database.Migrate();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();