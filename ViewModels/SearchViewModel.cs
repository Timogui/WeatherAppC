using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels;

public class SearchViewModel : ViewModelBase
{
    private readonly WeatherService _weatherService;
    private readonly OptionsService _optionsService;

    private string _cityInput = string.Empty;
    private string _cityName = string.Empty;
    private string _coordinates = string.Empty;
    private string _temperature = string.Empty;
    private string _description = string.Empty;
    private string _humidity = string.Empty;
    private string _iconUrl = string.Empty;
    private string _errorMessage = string.Empty;

    private bool _hasResult;
    private bool _isLoading;

    private IBrush _backgroundBrush = new SolidColorBrush(Colors.LightBlue);

    private Bitmap? _cityImage;

    public Bitmap? CityImage
    {
        get => _cityImage;
        set => SetProperty(ref _cityImage, value);
    }

    public string CityInput { get => _cityInput; set => SetProperty(ref _cityInput, value); }
    public string CityName { get => _cityName; set => SetProperty(ref _cityName, value); }
    public string Coordinates { get => _coordinates; set => SetProperty(ref _coordinates, value); }
    public string Temperature { get => _temperature; set => SetProperty(ref _temperature, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
    public string Humidity { get => _humidity; set => SetProperty(ref _humidity, value); }
    public string IconUrl { get => _iconUrl; set => SetProperty(ref _iconUrl, value); }
    public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }
    public string[] CitySuggestions { get; } = new[] 
    { 
        "Paris", "Lyon", "Marseille", "Toulouse", "Bordeaux", "Lille", "New York", "Tokyo", "Londres", "Montréal", "Genève"
    };

    public bool HasResult { get => _hasResult; set => SetProperty(ref _hasResult, value); }
    public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

    public IBrush BackgroundBrush
    {
        get => _backgroundBrush;
        set => SetProperty(ref _backgroundBrush, value);
    }

    public SearchViewModel(WeatherService weatherService, OptionsService optionsService)
    {
        _weatherService = weatherService;
        _optionsService = optionsService;

        if (!string.IsNullOrWhiteSpace(optionsService.Options.DefaultCity))
            CityInput = optionsService.Options.DefaultCity;
    }

    public async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(CityInput)) return;

        IsLoading = true;
        HasResult = false;
        ErrorMessage = string.Empty;

        try
        {
            var data = await _weatherService.GetCurrentWeatherAsync(
                CityInput, _optionsService.Options.Language);

            CityName = data.CityName;
            Coordinates = $"Lat: {data.Coord.Lat:F4} | Lon: {data.Coord.Lon:F4}";
            Temperature = $"{data.Main.Temp:F1} °C";
            Description = data.Weather.FirstOrDefault()?.Description ?? "-";
            Humidity = $"Humidité : {data.Main.Humidity} %";
            string iconCode = data.Weather.FirstOrDefault()?.Icon ?? "01d";
            IconUrl = WeatherService.GetIconUrl(iconCode);
            BackgroundBrush = iconCode switch
            {
                "01d" or "01n" => CreateWeatherGradient(Colors.LightBlue, Colors.DodgerBlue),
                "02d" or "03d" or "04d" or "04n" => CreateWeatherGradient(Colors.LightGray, Colors.SlateGray),
                "09d" or "10d" or "11d" => CreateWeatherGradient(Colors.SlateGray, Colors.DarkBlue),
                _ => CreateWeatherGradient(Color.Parse("#E0F7FA"), Color.Parse("#80DEEA"))
            };
            _ = FetchCityImageAsync(CityName);
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

    private IBrush CreateWeatherGradient(Color topColor, Color bottomColor)
    {
        return new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            GradientStops = new GradientStops
            {
                new GradientStop { Color = topColor, Offset = 0 },
                new GradientStop { Color = bottomColor, Offset = 1 }
            }
        };
    }

    private async Task FetchCityImageAsync(string cityName)
    {
        var configService = new ConfigService();
        string apiKey = configService.UnsplashApiKey; 
        
        string url = $"https://api.unsplash.com/search/photos?query={cityName}+city+landscape&client_id={apiKey}&orientation=landscape&per_page=1";

        using var client = new HttpClient();
        try
        {
            var jsonResponse = await client.GetStringAsync(url);
            var unsplashData = JsonConvert.DeserializeObject<UnsplashResponse>(jsonResponse);
            string? imageUrl = unsplashData?.Results?.FirstOrDefault()?.Urls?.Regular;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imageBytes = await client.GetByteArrayAsync(imageUrl);
                using var stream = new MemoryStream(imageBytes);
                CityImage = new Bitmap(stream);
            }
            else
            {
                CityImage = null;
            }
        }
        catch
        {
            CityImage = null;
        }
    }
}