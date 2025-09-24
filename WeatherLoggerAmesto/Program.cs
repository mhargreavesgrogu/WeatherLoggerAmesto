using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherLoggerAmesto;

//Möjliggör hosting av Azure Function Lokalt
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddLogging();
        services.AddScoped<IWeatherLoggerService, WeatherLoggerService>();
    })
    .Build();

host.Run();
