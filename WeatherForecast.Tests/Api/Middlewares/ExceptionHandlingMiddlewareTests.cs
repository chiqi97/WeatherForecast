using System.Globalization;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WeatherForecast.Shared.Exceptions;
using WeatherForecastAPI.Middlewares;

namespace WeatherForecast.Tests.Api.Middlewares;

[TestFixture]
public class ExceptionHandlingMiddlewareTests
{
    private RequestDelegate _nextDelegateMock = null!;
    private HttpContext _httpContext = null!;
    private ILogger<ExceptionHandlingMiddleware> _logger = null!;
    private ExceptionHandlingMiddleware _exceptionMiddleware = null!;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();
        _nextDelegateMock = Substitute.For<RequestDelegate>();
        _httpContext = new DefaultHttpContext();
        _exceptionMiddleware = new ExceptionHandlingMiddleware(_logger);
    }
    
    [Test]
    public async Task InvokeAsync_WhenNoException_ShouldInvokeNext()
    {
        //Arrange
        // Act
        await _exceptionMiddleware.InvokeAsync(_httpContext, _nextDelegateMock);

        // Assert
        await _nextDelegateMock.Received(1).Invoke(_httpContext);
    }
    
    [Test]
    public async Task InvokeAsync_WhenThrowApiException_ShouldReturnsBadRequest()
    {
        // Arrange
        var apiException = new ApiException( "message", "output");
        _nextDelegateMock.Invoke(Arg.Any<HttpContext>()).Throws(apiException);

        // Act
        await _exceptionMiddleware.InvokeAsync(_httpContext, _nextDelegateMock);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task InvokeAsync_WhenThrowEntityNotFoundException_ShouldReturnsNotFound()
    {
        // Arrange
        var ex = new EntityNotFoundException( "message");
        _nextDelegateMock.Invoke(Arg.Any<HttpContext>()).Throws(ex);

        // Act
        await _exceptionMiddleware.InvokeAsync(_httpContext, _nextDelegateMock);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
    
    [Test]
    public async Task InvokeAsync_WhenThrowSqliteException_ShouldReturnsServiceUnavailable()
    {
        // Arrange
        var ex = new SqliteException( "message", 1);
        _nextDelegateMock.Invoke(Arg.Any<HttpContext>()).Throws(ex);

        // Act
        await _exceptionMiddleware.InvokeAsync(_httpContext, _nextDelegateMock);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.ServiceUnavailable);
    }
    

}