
namespace WeatherLoggerAmesto
{
    //Kontrakt mot WeatherForecast-api
    public class Forecast
    {
        
        public DateTimeOffset Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }

    }
}
