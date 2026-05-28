using WeatherApp.Services;

namespace WeatherApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public SearchViewModel SearchVM { get; }
    public ForecastViewModel ForecastVM { get; }
    public SettingsViewModel SettingsVM { get; }

    public MainViewModel(WeatherService weatherService, OptionsService optionsService)
    {
        SearchVM = new SearchViewModel(weatherService, optionsService);
        ForecastVM = new ForecastViewModel(weatherService, optionsService);
        SettingsVM = new SettingsViewModel(optionsService);
    }
}
