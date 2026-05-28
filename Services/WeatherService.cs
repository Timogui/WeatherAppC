using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class WeatherService
{
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5";
    private readonly HttpClient _http = new();
    private readonly string _apiKey;

    public WeatherService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<WeatherData> GetCurrentWeatherAsync(string city, string lang = "fr")
    {
        var url = $"{BaseUrl}/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric&lang={lang}";
        return await FetchAsync<WeatherData>(url);
    }

    public async Task<List<ForecastItem>> GetForecastAsync(string city, string lang = "fr")
    {
        var url = $"{BaseUrl}/forecast?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric&lang={lang}";
        var response = await FetchAsync<ForecastResponse>(url);

        return response.List
            .Where(f => f.DateText.Contains("12:00:00"))
            .Take(5)
            .ToList();
    }

    public static string GetIconUrl(string iconCode)
        => $"https://openweathermap.org/img/wn/{iconCode}@2x.png";

    private async Task<T> FetchAsync<T>(string url)
    {
        HttpResponseMessage response;

        try
        {
            response = await _http.GetAsync(url);
        }
        catch (HttpRequestException)
        {
            throw new Exception("Impossible de se connecter. Vérifiez votre connexion internet.");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new Exception("Ville introuvable. Vérifiez le nom saisi.");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new Exception("Clé API invalide.");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json)
               ?? throw new Exception("Réponse invalide de l'API.");
    }
}
