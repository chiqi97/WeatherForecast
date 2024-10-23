using FluentAssertions;
using NSubstitute;
using WeatherForecast.Core.Clients;
using WeatherForecast.Core.Models.Clients.MeteoClient;
using WeatherForecast.Core.Models.WeatherForecast;
using WeatherForecast.Core.Services;
using WeatherForecast.Data.Repositories;

namespace WeatherForecast.Tests.Services;

public class WeatherForecastServiceTests
{
    private IWeatherRepository _weatherRepository;
    private IMeteoClient _meteoClient;
    private WeatherForecastService _weatherForecastService;

    [SetUp]
    public void Setup()
    {
        _weatherRepository = Substitute.For<IWeatherRepository>();
        _meteoClient = Substitute.For<IMeteoClient>();
        _weatherForecastService = new WeatherForecastService(_weatherRepository, _meteoClient);
    }
    
    [Test]
    public async Task AddWeatherForecastAndCoordinatesAsync_WhenCorrectRequest_ShouldCallRepositoryExactlyOnce()
    {
        // Arrange

        var expectedLat = 50.123m;
        var expectedLong = 8.45m;
        var expectedTemp = 25;
        var expectedInterval = 60;
        var expectedTime = TimeProvider.System.GetUtcNow().DateTime;
        var addWeatherForecast = new AddWeatherForecast
        {
            Latitude = expectedLat,
            Longitude = expectedLong
        };
        
        var meteoForecast = new MeteoWeatherForecast
        {
            Current = new CurrentWeather()
            {
                Time = expectedTime,
                Temperature2M = 25,
                Interval = 60
            }
        };

        _meteoClient.GetWeatherForecastAsync(addWeatherForecast).Returns(meteoForecast);

        _weatherRepository.AddOrUpdateAsync(Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<Data.Entities.WeatherForecast>())
            .Returns(1);

        // Act
        var result = await _weatherForecastService.AddWeatherForecastAndCoordinatesAsync(addWeatherForecast);

        // Assert
        await _weatherRepository.Received(1)
            .AddOrUpdateAsync(expectedLat, expectedLong, Arg.Is<Data.Entities.WeatherForecast>(wf =>
                wf.Temperature == expectedTemp &&
                wf.Interval == expectedInterval &&
                wf.Time == expectedTime));
    }
    
    [Test]
    public async Task GetByIdAsync_WhenExists_ShouldReturnWeatherForecastDtoAndCallRepoExactlyOnce()
    {
        // Arrange
        var inputId = 1;
        var weatherEntity = new Data.Entities.WeatherForecast
        {
            Id = inputId,
            Temperature = 25,
            Interval = 60,
            Time = DateTime.UtcNow,
            Coordinate = new Data.Entities.Coordinate
            {
                Id = 2,
                Latitude = 50.123m,
                Longitude = 8.456m
            }
        };

        _weatherRepository.GetById(1).Returns(weatherEntity);

        // Act
        var result = await _weatherForecastService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        await _weatherRepository.Received(1)
            .GetById(inputId);

    }
    
    [Test]
    public async Task DeleteAsync_WhenCorrect_ShouldCallRepositoryDelete()
    {
        // Arrange
        var expectedId = 1;
        _weatherRepository.DeleteAsync(1).Returns(expectedId);

        // Act
        var result = await _weatherForecastService.DeleteAsync(expectedId);

        // Assert
        result.Should().Be(1);
        await _weatherRepository.Received(1).DeleteAsync(expectedId);
    }
    
    

}