using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text.Json;

namespace WeatherLoggerAmesto
{
    public class WeatherLogger
    {
        private readonly ILogger<WeatherLogger> _logger;
        private readonly IWeatherLoggerService _weatherLoggerService;

        public WeatherLogger(ILogger<WeatherLogger> logger, IWeatherLoggerService weatherLoggerService)
        {
            _logger = logger;
            _weatherLoggerService = weatherLoggerService;
        }

        //Azure Fuction - tidsaktivterat varannan minut
        [Function("WeatherLogger")]
        public async Task Run([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer)
        {

            try
            {
                var forecasts = await _weatherLoggerService.GetWeatherAsync();

                //Null-check - om deserialisering misslyckades
                if (forecasts == null)
                {
                    _logger.LogWarning("Unable to parse response from Weather API");
                    return;
                }
                //Kontrollerar att listan inte är tom
                if (!forecasts.Any())
                {
                    _logger.LogWarning("The list of forecasts from the server is empty");
                    return;
                }

                //Loggar listans innehåll i önskat format
                foreach (var forecast in forecasts)
                {
                    _logger.LogInformation("Date: {forecast.date}, Temperature Celsius: {forecast.temperatureC}, Temperature Farenheit: {forecast.temperatureF}, Summary: {forecast.summary}",
                        forecast.Date.ToString("yyyy-MM-dd"), forecast.TemperatureC, forecast.TemperatureF, forecast.Summary);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Calling Weather API");
            }
        }
    }
}
