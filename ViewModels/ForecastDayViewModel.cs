using System;
using System.Linq;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels;

public class ForecastDayViewModel : ViewModelBase
{
    public string Date { get; }
    public string Temperature { get; }
    public string Description { get; }
    public string Humidity { get; }
    public string IconUrl { get; }

    public ForecastDayViewModel(ForecastItem item)
    {
        var exactDate = DateTime.Parse(item.DateText);
        
        Date = exactDate.ToString("ddd dd/MM\nHH:mm");
        
        Temperature = $"{item.Main.Temp:F1} °C";
        Description = item.Weather.FirstOrDefault()?.Description ?? "-";
        Humidity = $"Humidité : {item.Main.Humidity} %";
        IconUrl = WeatherService.GetIconUrl(item.Weather.FirstOrDefault()?.Icon ?? "01d");
    }
}