using System.Text.Json;

namespace EstateFresh.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    // Ballito, South Africa coordinates
    private const double Latitude = -29.5389;
    private const double Longitude = 31.2147;

    public WeatherService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenWeatherMap:ApiKey"] ?? "";
    }

    // Checks the 5-day forecast and returns true if rain is expected on the given date.
    public async Task<bool> IsRainExpectedAsync(DateOnly date)
    {
       
        if (string.IsNullOrEmpty(_apiKey))
            return false;

        var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={Latitude}&lon={Longitude}&appid={_apiKey}&units=metric";

        try
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var list = doc.RootElement.GetProperty("list");

            foreach (var entry in list.EnumerateArray())
            {
                var dtTxt = entry.GetProperty("dt_txt").GetString(); // e.g. "2026-10-30 12:00:00"
                if (dtTxt == null) continue;

                var entryDate = DateOnly.Parse(dtTxt.Split(' ')[0]);
                if (entryDate != date) continue;

                var weatherArray = entry.GetProperty("weather");
                foreach (var weather in weatherArray.EnumerateArray())
                {
                    var main = weather.GetProperty("main").GetString();
                    if (main == "Rain" || main == "Thunderstorm")
                        return true;
                }
            }

            return false;
        }
        catch
        {
            return false; // if the API call fails for any reason, fail safe - no warning shown
        }
    }
}