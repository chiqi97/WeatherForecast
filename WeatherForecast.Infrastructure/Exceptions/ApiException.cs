namespace WeatherForecast.Shared.Exceptions;

public class ApiException : Exception
{
    public string OutputMessage { get; set; }
    public ApiException(string systemMessage, string outputMessage) : base(systemMessage)
    {
        OutputMessage = outputMessage;
    }

    public ApiException(string message) : base(message)
    {
        
    }
}