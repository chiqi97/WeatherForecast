using FluentAssertions;
using Newtonsoft.Json;
using WeatherForecast.Core.Helpers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WeatherForecast.Tests.Core.Helpers;

[TestFixture]
public class JsonHelperTests
{
    private class TestClass
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
    }

    [Test]
    public void Deserialize_TestObject_ShouldMatchOriginal()
    {
        var testObject = new TestClass
        {
            Property1 = "Value1",
            Property2 = 42
        };

        var jsonHelper = new JsonHelper();
        var json = JsonSerializer.Serialize(testObject);
        
        var deserializedObject = jsonHelper.Deserialize<TestClass>(json);
        
        deserializedObject.Should().BeEquivalentTo(testObject);
    }
    

}