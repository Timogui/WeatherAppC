using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels;

public class ForecastViewModel : ViewModelBase
{
    private readonly WeatherService _weatherService;
    private readonly OptionsService _optionsService;
    private readonly ConfigService _configService;

    private string _cityInput = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading;
    private bool _hasResult;
    private Bitmap? _cityImage;

    public string CityInput
    {
        get => _cityInput;
        set => SetProperty(ref _cityInput, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool HasResult
    {
        get => _hasResult;
        set => SetProperty(ref _hasResult, value);
    }

    public Bitmap? CityImage
    {
        get => _cityImage;
        set => SetProperty(ref _cityImage, value);
    }

    public ObservableCollection<ForecastDayViewModel> Days { get; } = new();

    public ForecastViewModel(WeatherService weatherService, OptionsService optionsService)
    {
        _weatherService = weatherService;
        _optionsService = optionsService;
        _configService = new ConfigService();

        if (!string.IsNullOrWhiteSpace(optionsService.Options.DefaultCity))
            CityInput = optionsService.Options.DefaultCity;
    }

    public async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(CityInput)) return;

        IsLoading = true;
        HasResult = false;
        ErrorMessage = string.Empty;
        Days.Clear();

        try
        {
            var items = await _weatherService.GetForecastAsync(
                CityInput, _optionsService.Options.Language);

            var noonForecasts = items.Where(item => item.DateText.Contains("12:00:00")).Take(5);

            foreach (var item in noonForecasts)
                Days.Add(new ForecastDayViewModel(item));

            _ = FetchCityImageAsync(CityInput);

            HasResult = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task FetchCityImageAsync(string cityName)
    {
        string apiKey = _configService.UnsplashApiKey; 
        
        string query = Uri.EscapeDataString($"{cityName} city landscape");
        string url = $"https://api.unsplash.com/search/photos?query={query}&orientation=landscape&per_page=1";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {apiKey}");
        client.DefaultRequestHeaders.Add("Accept-Version", "v1");
        client.DefaultRequestHeaders.Add("User-Agent", "WeatherApp-Avalonia-Client");

        try
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var unsplashData = JsonConvert.DeserializeObject<UnsplashResponse>(jsonResponse);
            string? imageUrl = unsplashData?.Results?.FirstOrDefault()?.Urls?.Regular;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imageBytes = await client.GetByteArrayAsync(imageUrl);
                using var stream = new MemoryStream(imageBytes);
                var bitmap = new Bitmap(stream);
                Avalonia.Threading.Dispatcher.UIThread.Post(() => { CityImage = bitmap; });
            }
        }
        catch
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => { CityImage = null; });
        }
    }
}