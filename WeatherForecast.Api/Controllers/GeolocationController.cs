using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Core.Services;

namespace WeatherForecastAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class GeolocationController : ControllerBase
{
    private readonly IGeoLocationService _geoLocationService;

    public GeolocationController(IGeoLocationService geoLocationService)
    {
        _geoLocationService = geoLocationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPreviouslyUsed(int pageNumber = 1, int pageSize= 10)
    {
        var result = await _geoLocationService.GetPreviouslyUsed(pageNumber, pageSize);
        return Ok(result);
    }
}