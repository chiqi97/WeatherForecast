using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Entities;

namespace WeatherForecast.Tests.FakeDbContext;

public class FakeDbContext : WeatherForecastDbContext
{
    public FakeDbContext() 
        : base(new DbContextOptionsBuilder<WeatherForecastDbContext>()
            .UseInMemoryDatabase(databaseName: "TestWeatherForecastDb") 
            .Options)
    {
    }

    public void Seed()
    {
        Coordinates.Add(new Coordinate { Id = 1, Latitude = 10, Longitude = 20, LastRequestTime = new DateTime(2024, 10, 23, 14, 30, 0)});
        Coordinates.Add(new Coordinate { Id = 2,  Latitude = 30, Longitude = 40, LastRequestTime = new DateTime(2024, 10, 23, 14, 30, 0)});

        WeatherForecasts.Add(new Data.Entities.WeatherForecast
        {
            CoordinateId = 1,
            Time = new DateTime(2024, 10, 23, 15, 30, 0),
            Id = 1,
            Interval = 15,
            Temperature = 25.5
        });
        WeatherForecasts.Add(new Data.Entities.WeatherForecast
        {
            CoordinateId = 1,
            Time = new DateTime(2024, 10, 23, 15, 30, 0),
            Id = 2,
            Interval = 15,
            Temperature = 25.5
        });
        WeatherForecasts.Add(new Data.Entities.WeatherForecast
        {
            CoordinateId = 1,
            Time = new DateTime(2024, 10, 23, 16, 00, 0),
            Id = 3,
            Interval = 15,
            Temperature = 25.5
        });
        WeatherForecasts.Add(new Data.Entities.WeatherForecast
        {
            CoordinateId = 2,
            Time = new DateTime(2024, 10, 23, 16, 30, 0),
            Id = 4,
            Interval = 16,
            Temperature = 12.5
        });
        WeatherForecasts.Add(new Data.Entities.WeatherForecast
        {
            CoordinateId = 2,
            Time = new DateTime(2024, 10, 15, 13, 30, 0),
            Id = 5,
            Interval = 123,
            Temperature = 23.5
        });

        SaveChanges();
    }
}