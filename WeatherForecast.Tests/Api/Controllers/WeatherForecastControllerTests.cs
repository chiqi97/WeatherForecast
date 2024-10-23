using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WeatherForecast.Core.Models;
using WeatherForecast.Core.Models.Controllers;
using WeatherForecast.Core.Models.WeatherForecast;
using WeatherForecast.Core.Services;
using WeatherForecastAPI.Controllers;

namespace WeatherForecast.Tests.Api.Controllers;

[TestFixture]
public class WeatherForecastControllerTests
{
    private WeatherForecastController _controller;
    private IWeatherForecastService _weatherForecastService;

    [SetUp]
    public void SetUp()
    {
        _weatherForecastService = Substitute.For<IWeatherForecastService>();
        _controller = new WeatherForecastController(_weatherForecastService);
    } 
    
    [Test]
    public async Task Add_WhenValidAddWeatherForecast_ShouldReturnOkWithId()
    {
        // Arrange
        var addWeatherForecast = new AddWeatherForecast { Longitude = 10, Latitude = 50, Current = CurrentTemperature.temperature_2m};
        var expectedId = 1;
        _weatherForecastService.AddWeatherForecastAndCoordinatesAsync(addWeatherForecast).Returns(expectedId);

        // Act
        var result = await _controller.Add(addWeatherForecast) as OkObjectResult;

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        var idResponse = result.Value as IdResponse;
        idResponse.Id.Should().Be(expectedId);
    }

    [Test]
    public async Task Delete_WhenValidId_ShouldReturnNoContent()
    {
        // Arrange
        var id = 1;

        // Act
        var result = await _controller.Delete(id) as NoContentResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        await _weatherForecastService.Received(1).DeleteAsync(id);
    }

    [Test]
    public async Task Get_WhenValidId_ShouldReturnOkWithWeatherForecastDto()
    {
        // Arrange
        var id = 1;
        var expectedWeatherForecast = new WeatherForecastDto {  };
        _weatherForecastService.GetByIdAsync(id).Returns(expectedWeatherForecast);

        // Act
        var result = await _controller.Get(id) as OkObjectResult;

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        var weatherForecastDto = result.Value as WeatherForecastDto;
        weatherForecastDto.Should().BeEquivalentTo(expectedWeatherForecast);
    }
}