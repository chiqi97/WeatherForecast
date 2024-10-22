namespace WeatherForecast.Core.Models.Controllers;

public class IdResponse
{
    public IdResponse(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}