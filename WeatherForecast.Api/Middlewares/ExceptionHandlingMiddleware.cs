using System.Net;
using Microsoft.Data.Sqlite;
using WeatherForecast.Shared.Exceptions;

namespace WeatherForecastAPI.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (ApiException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(e.OutputMessage);
        }
        catch (EntityNotFoundException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(e.Message);
        }
        catch (SqliteException e)
        {
            _logger.LogError(e, $"[SqliteException] - {e.Message}");
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            await context.Response.WriteAsync("We have problems with connection to database!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}