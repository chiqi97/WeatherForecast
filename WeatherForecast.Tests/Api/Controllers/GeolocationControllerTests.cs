using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WeatherForecast.Core.Models.GeoLocation;
using WeatherForecast.Core.Services;
using WeatherForecastAPI.Controllers;

namespace WeatherForecast.Tests.Api.Controllers;

public class GeolocationControllerTests
{
    private IGeoLocationService _geoLocationService;
    private GeolocationController _controller;

    [SetUp]
    public void Setup()
    {
        _geoLocationService = Substitute.For<IGeoLocationService>();
        _controller = new GeolocationController(_geoLocationService);
    }
    
    [Test]
    public async Task GetPreviouslyUsed_WhenServiceReturnList_ShouldReturnOkAndCorrectresult()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var geoLocationDtos = new List<GeoLocationDto> { new GeoLocationDto(){Coordinate = new GeoLocationCoordinateDto(){Id = 1, LastRequestTime = TimeProvider.System.GetUtcNow().DateTime, Latitude = 10, Longitude = 10}, 
            WeatherForecast = new GeoLocationWeatherForecastDto() {Interval = 100, Temperature = 25.5, Id = 5, Time = TimeProvider.System.GetUtcNow().DateTime}}};

        _geoLocationService.GetPreviouslyUsedAsync(pageNumber, pageSize).Returns(Task.FromResult((IList<GeoLocationDto>)geoLocationDtos));

        // Act
        var result = await _controller.GetPreviouslyUsed(pageNumber, pageSize) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(geoLocationDtos);
    }
    [Test]
    public async Task GetPreviouslyUsed_WhenServiceReturnsEmptyList_ShouldReturnOkResultWithEmptyList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var emptyResult = new GeoLocationDto[] { };

        _geoLocationService.GetPreviouslyUsedAsync(pageNumber, pageSize).Returns(Task.FromResult((IList<GeoLocationDto>)emptyResult));

        // Act
        var result = await _controller.GetPreviouslyUsed(pageNumber, pageSize) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(emptyResult);
    }
    
    [Test]
    public async Task GetPreviouslyUsed_WhenCorrectRequest_ShouldCallGeoLocationServiceWithCorrectParametersExactlyOnce()
    {
        // Arrange
        var pageNumber = 2;
        var pageSize = 5;

        // Act
        await _controller.GetPreviouslyUsed(pageNumber, pageSize);

        // Assert
        await _geoLocationService.Received(1).GetPreviouslyUsedAsync(pageNumber, pageSize);
    }
}