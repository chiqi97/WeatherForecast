namespace WeatherForecast.Shared.Exceptions;

public class EntityNotFoundException : KeyNotFoundException
{
    public EntityNotFoundException(string message) : base(message)
    {
        
    }
    
}