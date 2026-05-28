using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WeatherApp.Services;
using WeatherApp.ViewModels;
using WeatherApp.Views;

namespace WeatherApp;

public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            try
            {
                var config = new ConfigService();
                var options = new OptionsService();
                var weather = new WeatherService(config.ApiKey);

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(weather, options)
                };
            }
            catch (Exception ex)
            {
                var errorWindow = new Avalonia.Controls.Window
                {
                    Title = "Erreur de configuration",
                    Width = 500,
                    Height = 160,
                    Content = new Avalonia.Controls.TextBlock
                    {
                        Text = ex.Message,
                        Margin = new Avalonia.Thickness(20),
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap
                    }
                };
                desktop.MainWindow = errorWindow;
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}