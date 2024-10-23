using FluentAssertions;
using Microsoft.Data.Sqlite;
using MockQueryable;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WeatherForecast.Core.Services;
using WeatherForecast.Data.Entities;
using WeatherForecast.Data.Repositories;

namespace WeatherForecast.Tests.Services;

[TestFixture]
public class GeoLocationServiceTests
{
    private ICoordinateRepository _coordinateRepository;
    private GeoLocationService _geoLocationService;

    [SetUp]
    public void Setup()
    {
        _coordinateRepository = Substitute.For<ICoordinateRepository>();
        _geoLocationService = new GeoLocationService(_coordinateRepository);
    }

    [Test]
    public async Task GetPreviouslyUsedAsync_WhenCoordinatesExists_ShouldReturnGeoLocationDtos()
    {
        // Arrange
        var coordinates = new List<Coordinate>
        {
            new Coordinate
            {
                Id = 1,
                Latitude = 50.123m,
                Longitude = 8.456m,
                LastRequestTime = DateTime.UtcNow,
                WeatherForecasts = new List<Data.Entities.WeatherForecast>
                {
                    new Data.Entities.WeatherForecast {Id = 1, Temperature = 25, Time = DateTime.UtcNow, Interval = 60}
                }
            }
        }.AsQueryable().BuildMock();

        _coordinateRepository.GetQuery().Returns(coordinates);

        // Act
        var result = await _geoLocationService.GetPreviouslyUsedAsync(1, 10);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.First().Coordinate.Latitude.Should().Be(50.123m);
        result.First().Coordinate.Longitude.Should().Be(8.456m);
        result.First().WeatherForecast.Should().NotBeNull();
        result.First().WeatherForecast.Temperature.Should().Be(25);
    }
    
    [Test]
    public async Task GetPreviouslyUsedAsync_WhenTableIsEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyCoordinates = new List<Coordinate>().AsQueryable().BuildMock();
        _coordinateRepository.GetQuery().Returns(emptyCoordinates);

        // Act
        var result = await _geoLocationService.GetPreviouslyUsedAsync(1, 10);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Test]
    public async Task GetPreviouslyUsedAsync_WhenCorrectCall_ShouldReturnPaginatedResults()
    {
        // Arrange
        var expectedFirstCoordinateId = 2;
        var coordinates = new List<Coordinate>
        {
            new Coordinate { Id = 1, Latitude = 50.123m, Longitude = 8.456m, LastRequestTime = TimeProvider.System.GetUtcNow().DateTime.AddHours(-2),       WeatherForecasts = new List<Data.Entities.WeatherForecast>
            {
                new Data.Entities.WeatherForecast {Id = 1, Temperature = 25, Time = DateTime.UtcNow, Interval = 60}
            }},
            new Coordinate
            {
                Id = expectedFirstCoordinateId, Latitude = 52.123m, Longitude = 9.456m, LastRequestTime = TimeProvider.System.GetUtcNow().DateTime.AddHours(-1),
                WeatherForecasts = new List<Data.Entities.WeatherForecast>
                {
                    new Data.Entities.WeatherForecast {Id = 1, Temperature = 25, Time = DateTime.UtcNow, Interval = 60}
                }
            }
        }.AsQueryable().BuildMock();

        _coordinateRepository.GetQuery().Returns(coordinates);

        // Act
        var result = await _geoLocationService.GetPreviouslyUsedAsync(1, 1);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.First().Coordinate.Id.Should().Be(expectedFirstCoordinateId);
    }
    
    [Test]
    public async Task GetPreviouslyUsedAsync_WhenCorrectRequest_ShouldReturnMostRecentCoordinatesFirst()
    {
        // Arrange
        var newestCoordinateId = 2;
        var coordinates = new List<Coordinate>
        {
            new Coordinate
            {
                Id = 1, Latitude = 50.123m, Longitude = 8.456m, LastRequestTime = TimeProvider.System.GetUtcNow().DateTime.AddHours(-2),
                WeatherForecasts = new List<Data.Entities.WeatherForecast>
                {
                    new Data.Entities.WeatherForecast {Id = 1, Temperature = 25, Time = DateTime.UtcNow, Interval = 60}
                }
            },
            new Coordinate
            {
                Id = newestCoordinateId, Latitude = 52.123m, Longitude = 9.456m, LastRequestTime = TimeProvider.System.GetUtcNow().DateTime.AddHours(-1),
                WeatherForecasts = new List<Data.Entities.WeatherForecast>
                {
                    new Data.Entities.WeatherForecast {Id = 1, Temperature = 25, Time = DateTime.UtcNow, Interval = 60}
                }
            }
        }.AsQueryable().BuildMock();

        _coordinateRepository.GetQuery().Returns(coordinates);

        // Act
        var result = await _geoLocationService.GetPreviouslyUsedAsync(1, 10);

        // Assert
        result.Should().NotBeNull();
        result.First().Coordinate.Id.Should().Be(2);
    }
    
    [Test]
    public async Task GetPreviouslyUsedAsync_WhenDbThrowException_ShouldThrowSqliteException()
    {
        // Arrange


        _coordinateRepository.GetQuery().Should().Throws(new SqliteException("", 1));

        // Act
        var act = async () => await _geoLocationService.GetPreviouslyUsedAsync(1, 10);

        // Assert
        await act.Should().ThrowAsync<SqliteException>();
    }
}