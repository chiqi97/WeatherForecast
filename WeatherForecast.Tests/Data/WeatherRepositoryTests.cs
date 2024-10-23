using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.Repositories;
using WeatherForecast.Shared.Exceptions;

namespace WeatherForecast.Tests.Core;

[TestFixture]
public class WeatherRepositoryTests
{
    private FakeDbContext.FakeDbContext _dbContext;
    private WeatherRepository _weatherRepository;

    [SetUp]
    public void Setup()
    {
        _dbContext = new FakeDbContext.FakeDbContext();
        _dbContext.Seed(); 
        _weatherRepository = new WeatherRepository(_dbContext);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
     [Test]
    public async Task AddOrUpdateAsync_WhenCoordinateDoesNotExist_ShouldAddNewWeatherForecastWithCoordinates()
    {
        // Arrange
        var latitude = 15m;
        var longitude = 25m;
        var newWeatherForecast = new Data.Entities.WeatherForecast
        {
            Time = DateTime.Now,
            Temperature = 22,
            Interval = 15,
            CoordinateId = 1
        };

        // Act
        var result = await _weatherRepository.AddOrUpdateAsync(latitude, longitude, newWeatherForecast);

        // Assert
        result.Should().BeGreaterThan(0);
        var addedForecast = _dbContext.WeatherForecasts.Include(w => w.Coordinate).FirstOrDefault(w => w.Id == result);
        addedForecast.Should().NotBeNull();
        addedForecast.Coordinate.Latitude.Should().Be(latitude);
        addedForecast.Coordinate.Longitude.Should().Be(longitude);
    }
    
    [Test]
    public async Task AddOrUpdateAsync_WhenCoordinateExists_ShouldUpdateExistingCoordinatesAndAddWeatherForecast()
    {
        // Arrange
        var coordinateId = 1;
        var existingCoordinate = _dbContext.Coordinates.First(x=> x.Id == 1);
        var latitude = existingCoordinate.Latitude;
        var longitude = existingCoordinate.Longitude;
        var newWeatherForecast = new Data.Entities.WeatherForecast
        {
            Time = DateTime.Now,
            Temperature = 22,
            Interval = 15,
            CoordinateId = coordinateId
        };

        // Act
        var result = await _weatherRepository.AddOrUpdateAsync(latitude, longitude, newWeatherForecast);

        // Assert
        result.Should().BeGreaterThan(0);
        var updatedCoordinate = _dbContext.Coordinates.First(c => c.Id == existingCoordinate.Id);
        updatedCoordinate.LastRequestTime.Should().BeCloseTo(TimeProvider.System.GetUtcNow().DateTime, TimeSpan.FromSeconds(1));
        newWeatherForecast.CoordinateId.Should().Be(existingCoordinate.Id);
    }
    [Test]
    public async Task GetById_WhenExists_ShouldReturnWeatherForecastWithCoordinate()
    {
        // Arrange
        var existingWeatherForecast = _dbContext.WeatherForecasts.Include(w => w.Coordinate).First();

        // Act
        var result = await _weatherRepository.GetById(existingWeatherForecast.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(existingWeatherForecast.Id);
        result.Coordinate.Should().NotBeNull();
    }

    [Test]
    public void GetById_WhenWeatherForecastDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var nonExistentId = 999;

        // Act
        var act = async () => await _weatherRepository.GetById(nonExistentId);

        // Assert
        act.Should().ThrowAsync<EntityNotFoundException>();
    }
    
    [Test]
    public async Task DeleteAsync_WhenExists_ShouldRemoveWeatherForecast()
    {
        // Arrange
        var existingWeatherForecast = _dbContext.WeatherForecasts.First();

        // Act
        var result = await _weatherRepository.DeleteAsync(existingWeatherForecast.Id);

        // Assert
        result.Should().Be(1);
        var deletedWeatherForecast = await _dbContext.WeatherForecasts.FindAsync(existingWeatherForecast.Id);
        deletedWeatherForecast.Should().BeNull();
    }
    
    [Test]
    public async Task DeleteAsync_WhenDoesNotExists_ShouldThrowEntityNotFoundEception()
    {
        // Arrange
        var notExistingId = 1231231;

        // Act
        var act = async () => await _weatherRepository.DeleteAsync(notExistingId);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
   
    
}