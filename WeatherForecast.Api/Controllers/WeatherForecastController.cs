using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Core.Models.Controllers;
using WeatherForecast.Core.Models.WeatherForecast;
using WeatherForecast.Core.Services;

namespace WeatherForecastAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(AddWeatherForecast addWeatherForecast)
    {
        var result = await _weatherForecastService.AddWeatherForecastAndCoordinatesAsync(addWeatherForecast);
        return Ok(new IdResponse(result));
    }

    [HttpDelete]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        await _weatherForecastService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WeatherForecastDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _weatherForecastService.GetByIdAsync(id);
        return Ok(result);
    }
}