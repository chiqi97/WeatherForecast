using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using RestSharp;
using RichardSzalay.MockHttp;
using WeatherForecast.Core.Clients;
using WeatherForecast.Core.Clients.Configuration;
using WeatherForecast.Core.Clients.Providers;
using WeatherForecast.Core.Helpers;
using WeatherForecast.Core.Models;
using WeatherForecast.Core.Models.Clients.MeteoClient;
using WeatherForecast.Core.Models.WeatherForecast;
using WeatherForecast.Shared.Exceptions;

namespace WeatherForecast.Tests.Core.Clients;

public class MeteoClientTests
{
    private IJsonHelper _jsonHelper = null!;
    private IMeteoClient _meteoClient = null!;
    private IRestClientProvider<MeteoConfiguration> _restClientProvider = null!;
    private IOptions<MeteoConfiguration> _options = null!;
    private const string BaseUrl = "http://localhost/";
    private const string GetSuccessPath = "api/get";
    private const string GetErrorPath = "api/get/Error";
    
    [SetUp]
    public void Setup()
    {
        var options = Substitute.For<IOptions<MeteoConfiguration>>();
        options.Value.Returns(new MeteoConfiguration() {Host = BaseUrl, GetWeatherForecastPath = GetSuccessPath});
        _options = options;
        _jsonHelper = Substitute.For<IJsonHelper>();

        _restClientProvider = Substitute.For<IRestClientProvider<MeteoConfiguration>>();

        var setupMockRestClient = SetupMockRestClient();
        _restClientProvider.Get().Returns(setupMockRestClient);

        _meteoClient = new MeteoClient(_restClientProvider, _jsonHelper, options);
        
    }

    [Test]
    public async Task GetWeatherForecastAsync_WhenCorrectResponse_ShouldReturnMeteoWeatherForecastModel()
    {
        //Arrange
        var inputeModel = new AddWeatherForecast()
            {Longitude = 50, Latitude = 10, Current = CurrentTemperature.temperature_2m};
        _jsonHelper.Deserialize<MeteoWeatherForecast>(SampleOpenMeteoResponseJson()).Returns(new MeteoWeatherForecast()
            {Current = new CurrentWeather() {Interval = 10, Time = TimeProvider.System.GetUtcNow().DateTime}});
        //Act
        var result = await _meteoClient.GetWeatherForecastAsync(inputeModel);
        
        //Assert
        result.Should().NotBeNull();
    }
    
    [Test]
    public async Task GetWeatherForecastAsync_WhenInCorrectResponse_ShouldThrowApiException()
    {
        //Arrange
        var inputeModel = new AddWeatherForecast()
            {Longitude = 50, Latitude = 10, Current = CurrentTemperature.temperature_2m};
        _jsonHelper.Deserialize<MeteoWeatherForecast>(SampleOpenMeteoResponseJson()).Returns(new MeteoWeatherForecast()
            {Current = new CurrentWeather() {Interval = 10, Time = TimeProvider.System.GetUtcNow().DateTime}});
        _options.Value.Returns(new MeteoConfiguration() {Host = BaseUrl, GetWeatherForecastPath = GetErrorPath});
        _meteoClient = new MeteoClient(_restClientProvider, _jsonHelper, _options);
        
        //Act
        var act = async () => await _meteoClient.GetWeatherForecastAsync(inputeModel);
        
        //Assert
        await act.Should().ThrowAsync<ApiException>();
    }
    
    
    private RestClient SetupMockRestClient()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When(BaseUrl + GetSuccessPath)
            .Respond("application/json", SampleOpenMeteoResponseJson());
        
        mockHttp.When(BaseUrl + GetErrorPath)
            .Respond(HttpStatusCode.InternalServerError, "application/json", "");


        var options = new RestClientOptions(new Uri(_options.Value.Host))
        {
            ConfigureMessageHandler = _ => mockHttp,
        };
        var client = new RestClient(options);

        return client;
    }
    
    private static string SampleOpenMeteoResponseJson()
    {
        return $@"
                {{
                ""latitude"": 18.625,
                ""longitude"": 54.375,
                ""generationtime_ms"": 0.015974044799804688,
                ""utc_offset_seconds"": 0,
                ""timezone"": ""GMT"",
                ""timezone_abbreviation"": ""GMT"",
                ""elevation"": 229,
                ""current_units"": {{
                ""time"": ""iso8601"",
                ""interval"": ""seconds"",
                ""temperature_2m"": ""°C""
                }},
                ""current"": {{
                ""time"": ""2024-10-23T15:30"",
                ""interval"": 900,
                ""temperature_2m"": 30.9
                }}
                }}";
    }
}


