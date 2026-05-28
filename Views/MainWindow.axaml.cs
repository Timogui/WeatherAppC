using Avalonia.Controls;
using Avalonia.Input;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class MainWindow : Window
{
    private MainViewModel VM => (MainViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void SearchButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => await VM.SearchVM.SearchAsync();

    private async void SearchBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            await VM.SearchVM.SearchAsync();
    }

    private async void ForecastButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => await VM.ForecastVM.SearchAsync();

    private async void ForecastBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            await VM.ForecastVM.SearchAsync();
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => VM.SettingsVM.Save();
}
