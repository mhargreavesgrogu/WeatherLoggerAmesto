namespace WeatherLoggerAmesto

{
    public interface IWeatherLoggerService 
    { 
        public Task<List<Forecast>?> GetWeatherAsync();
    }

}
