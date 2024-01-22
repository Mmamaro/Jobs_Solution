using System.Net.Http;
using System.Text.Json;

namespace AssessmentAPI.Service
{
    public class WeatherDatafetcher
    {
        private readonly HttpClient _httpClient;

        public WeatherDatafetcher(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Dictionary<string, string>> FetchWeatherDataAsync(string city)
        {
            Dictionary<string, string> weatherData = new Dictionary<string, string>();


            var apiKey = "dc1b377e952d035bd7ee3a39a1f394df";

            //Making the request and saving the results on response
            using HttpResponseMessage response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&APPID={apiKey}");

            //Making sure it brings back a successful response
            response.EnsureSuccessStatusCode();

            //Making the response a json format
            var jsonResponse = await response.Content.ReadAsStringAsync();

            //Getting the temperature from the json response which happens to be an object
            using JsonDocument jsonDoc = JsonDocument.Parse(jsonResponse);

            if (jsonDoc.RootElement.TryGetProperty("main", out JsonElement mainElement))
            {
                if (mainElement.TryGetProperty("temp", out JsonElement tempElement))
                {
                    string temperature = tempElement.GetDouble().ToString();

                    weatherData["Temperature"] = temperature;

                    //Console.WriteLine($"temperature: {temperature}");
                }
            }


            //Getting the wind from the json response which happens to be an object

            if (jsonDoc.RootElement.TryGetProperty("wind", out JsonElement windElement))
            {
                if (windElement.TryGetProperty("speed", out JsonElement speedElement))
                {
                    string windSpeed = speedElement.GetDouble().ToString();
                    weatherData["WindSpeed"] = windSpeed;

                    //Console.WriteLine($"Wind Speed: {windSpeed}");
                }
            }




            //Getting the description from the json response which happens to be an array of objects

            if (jsonDoc.RootElement.TryGetProperty("weather", out JsonElement weatherElement))
            {

                if (weatherElement.ValueKind == JsonValueKind.Array && weatherElement.EnumerateArray().Any())
                {

                    JsonElement firstWeather = weatherElement.EnumerateArray().First();

                    if (firstWeather.TryGetProperty("description", out JsonElement descriptionElement))
                    {
                        string description = descriptionElement.GetString();

                        weatherData["Description"] = description;

                        //Console.WriteLine($"Description: {description}");
                    }
                }

            }

            return weatherData;


        }

    }

}
