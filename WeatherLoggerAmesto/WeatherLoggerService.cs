using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherLoggerAmesto

{
    public class WeatherLoggerService : IWeatherLoggerService
    {
        private readonly HttpClient _httpClient;

        public WeatherLoggerService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<List<Forecast>?> GetWeatherAsync()
        {
            //Skapar GET-request mot API
            var request = new HttpRequestMessage(HttpMethod.Get, "https://swetest-dwefa3gva8erbchn.swedencentral-01.azurewebsites.net/WeatherForecast");

            // Adderar HTTP-header för autentisering 
            // API-nyckel exponeras, vid publicering använd verktyg likt Azure Key Vault med en MSI
            var API_KEY = "";
            request.Headers.Add("x-api-key", API_KEY);

            //Adderar önskat responseformat JSON 
            request.Headers.Add("Accept", "application/json");

            //Tar emot response och validerar statuskod
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //Extraherar JSON-sträng och deserialiserar till lista av strukturen i Forecast.cs
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Forecast>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

}
