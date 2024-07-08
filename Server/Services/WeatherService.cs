using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using Newtonsoft.Json.Linq;
using ServerLibrary.Repositories.Contracts;

namespace Server.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private string apiKey = "9bd7e4fdc4eb4937b1a9deb743816aab";
        private string openApi = "8d9e7d33074f30d15e4fbc345efa073f";

        public async Task<object> GetForecastData(WeatherDTO weather)
        {
            try
            {
                var url = "https://api.openweathermap.org/geo/1.0/direct";
                var uriBuilder = new UriBuilder(url);
                var query = $"q={weather.region},{weather.country}&limit=1&appid={openApi}";
                uriBuilder.Query = query;
                // Step 1: Get latitude and longitude using the Geocoding API
                var geoResponse = await _httpClient.GetAsync(uriBuilder.Uri);

                if (!geoResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to fetch geolocation. Please check the country and region name and try again.");
                }

                var geoContent = await geoResponse.Content.ReadAsStringAsync();
                var geoData = JArray.Parse(geoContent);

                if (geoData?.Count == 0)
                {
                    throw new Exception("Failed to fetch geolocation. Please check the country and region name and try again.");
                }

                var lat = (double)geoData[0]["lat"];
                var lon = (double)geoData[0]["lon"];

                // Step 2: Fetch weather data using the latitude and longitude
                var weatherResponse = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={apiKey}");

                if (!weatherResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to fetch weather data. Please check the country and region name and try again.");
                }

                var weatherContent = await weatherResponse.Content.ReadAsStringAsync();
                var weatherDataList = JObject.Parse(weatherContent)["list"];

                var filteredWeatherData = new List<WeatherData>();

                foreach (var entry in weatherDataList)
                {
                    var entryTimestamp = (long)entry["dt"];
                    var entryDateTime = DateTimeOffset.FromUnixTimeSeconds(entryTimestamp).DateTime;
                    var entryDate = DateOnly.FromDateTime(entryDateTime);

                    if (entryDate >= weather.startDate && entryDate <= weather.endDate)
                    {
                        filteredWeatherData.Add(new WeatherData
                        {
                            Date = entryDateTime.ToString("yyyy-MM-dd"),
                            Time = entryDateTime.ToString("HH:mm:ss"),
                            Weather = entry["weather"][0]["description"].ToString(),
                            Temperature = $"{(double)entry["main"]["temp"] / 10:0.00}°C"
                        });
                    }
                }

                return filteredWeatherData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch weather data: {ex.Message}");
                return new List<WeatherData>();
            }
        }

        public async Task<object> GetHistoricalData(WeatherDTO weather)
        {
            var location = weather.region + ", " + weather.region;
            var response = await _httpClient.GetAsync($"https://api.weather.com/v1/historical?location={location}&startDate={weather.startDate}&endDate={weather.endDate}&apikey={openApi}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        public async Task<object> FetchWeatherDataForYear(DateTime startDate, DateTime endDate, double lat, double lon)
        {
            string FormatDate(DateTime date) => date.ToString("yyyy-MM-dd");

            string GetCloudDescription(int cloudPercentage)
            {
                if (cloudPercentage <= 25)
                {
                    return "clear sky";
                }
                else if (cloudPercentage <= 50)
                {
                    return "scattered clouds";
                }
                else if (cloudPercentage <= 75)
                {
                    return "broken clouds";
                }
                else
                {
                    return "overcast clouds";
                }
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://api.weatherbit.io/v2.0/history/daily?lat={lat}&lon={lon}&start_date={FormatDate(startDate)}&end_date={FormatDate(endDate)}&key={apiKey}");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<object>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(content)["data"];

                var weatherData = new List<object>();
                foreach (var item in data)
                {
                    weatherData.Add(new
                    {
                        Date = item["datetime"].ToString(),
                        Weather = GetCloudDescription((int)item["clouds"]),
                        Temperature = item["temp"].ToString()
                    });
                }

                return weatherData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch historical weather data: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<object> FetchHistoricalWeatherData(string country, string region, DateTime startDate, DateTime endDate)
        {
            try
            {
                var geoResponse = await _httpClient.GetAsync($"https://api.openweathermap.org/geo/1.0/direct?q={region},{country}&limit=1&appid={openApi}");

                if (!geoResponse.IsSuccessStatusCode)
                {
                    return new List<object>();
                }

                var geoContent = await geoResponse.Content.ReadAsStringAsync();
                var geoData = JArray.Parse(geoContent);

                if (geoData.Count == 0)
                {
                    throw new Exception("Failed to fetch geolocation. Please check the country and region name and try again.");
                }

                var lat = (double)geoData[0]["lat"];
                var lon = (double)geoData[0]["lon"];
                var weatherData = new List<object>();

                for (int year = startDate.Year - 1; year >= startDate.Year - 8; year--)
                {
                    var newStartDate = new DateTime(year, startDate.Month, startDate.Day);
                    var newEndDate = new DateTime(year, endDate.Month, endDate.Day).AddDays(1);

                    var dataForYear = await FetchWeatherDataForYear(newStartDate, newEndDate, lat, lon);
                    weatherData.AddRange(dataForYear as List<object>);
                }

                return weatherData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch weather data: {ex.Message}");
                return new List<object>();
            }
        }
    }

    public class WeatherData
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Weather { get; set; }
        public string Temperature { get; set; }
    }
}
