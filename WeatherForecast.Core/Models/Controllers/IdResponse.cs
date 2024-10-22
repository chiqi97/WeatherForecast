namespace WeatherForecast.Core.Models.Controllers;

public record IdResponse
{
    public IdResponse(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}