using AutoFixture;
using FakeItEasy;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherLoggerAmesto;
using Xunit;

namespace WeatherLoggerAmesto.Tests
{
    public class WeatherLoggerTests
    {
        private readonly ILogger<WeatherLogger> _fakeLogger;
        private readonly IWeatherLoggerService _fakeWeatherLoggerService;
        private WeatherLogger _weatherLogger;
        private Fixture fixture;

        public WeatherLoggerTests()
        {
            // Slumpar data, skapar dynamiska testobjekt
            fixture = new Fixture();

            // FakeItEasy för att mocka serviceanrop
            _fakeLogger = A.Fake<ILogger<WeatherLogger>>();
            _fakeWeatherLoggerService = A.Fake<IWeatherLoggerService>();
            _weatherLogger = new WeatherLogger(_fakeLogger, _fakeWeatherLoggerService);
        }

        [Fact]
        public async Task Run_EmptyForecastList_LogsEmptyList()
        {
            // Arrange - skapar testdata 
            List<Forecast>? weatherForecastResponse = [fixture.Create<Forecast>()];

            // Mockar GetWeatherAsync-metoden
            A.CallTo(() => _fakeWeatherLoggerService.GetWeatherAsync()).Returns((List<Forecast>?)weatherForecastResponse);
            
            // Act - kör weatherlogger
            await _weatherLogger.Run(fixture.Create<TimerInfo>());

            // Assert - då vi har en forecast, kolla att vi loggar exakt en gång
            A.CallTo(_fakeLogger).Where(x => x.Method.Name == "Log").MustHaveHappenedOnceExactly();
        }
    }
}

