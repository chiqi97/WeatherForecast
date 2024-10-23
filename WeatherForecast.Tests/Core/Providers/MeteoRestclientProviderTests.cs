using FluentAssertions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NSubstitute;
using WeatherForecast.Core.Clients.Configuration;
using WeatherForecast.Core.Clients.Providers;

namespace WeatherForecast.Tests.Core;

public class MeteoRestClientProviderTests
{
    private MeteoConfiguration _configuration = null!;
    private MeteoRestClientProvider _restClientProvider = null!;
    private const string HostUrl = "https://example.com/api";

    [SetUp]
    public void Setup()
    {
         var options = Substitute.For<IOptions<MeteoConfiguration>>();
         options.Value.Returns(new MeteoConfiguration() {Host = HostUrl, GetWeatherForecastPath = "example/path"});
        _restClientProvider = new MeteoRestClientProvider(options);
    }
    
    [Test]
    public void RestClientProvider_WhenCallGet_ShouldReturnCorrectRestClient()
    {
        // Arrange

        // Act
        var restClient = _restClientProvider.Get();

        // Assert
        restClient.Options.BaseUrl.Should().Be(HostUrl);
    }
    
}