using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Entities;
using WeatherForecast.Data.Repositories;

namespace WeatherForecast.Tests.Core;

[TestFixture]
public class CoordinateRepositoryTests
{
    private FakeDbContext.FakeDbContext _dbContext;
    private CoordinateRepository _coordinateRepository;

    [SetUp]
    public void Setup()
    {
        _dbContext = new FakeDbContext.FakeDbContext();
        _dbContext.Seed();
        _coordinateRepository = new CoordinateRepository(_dbContext);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    [Test]
    public void GetQuery_WhenSendRequest_ShouldReturnQueryableCoordinates()
    {
        //Arrange
        
        // Act
        var result = _coordinateRepository.GetQuery();

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }
}