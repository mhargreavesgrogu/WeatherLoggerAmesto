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
            // API-nyckel exponeras, vid publicering av kritisk API-nyckel använd verktyg likt Azure Key Vault med en MSI
            var API_KEY = "b7e8e2e2-2e2a-4e4b-9c8e-7a2e1d3f4b5c";
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
