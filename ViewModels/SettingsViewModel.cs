using System.Collections.ObjectModel;
using System.Linq;
using WeatherApp.Services;

namespace WeatherApp.ViewModels;

public class LanguageOption
{
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public override string ToString() => Label;
}

public class SettingsViewModel : ViewModelBase
{
    private readonly OptionsService _optionsService;

    private string _defaultCity = string.Empty;
    private LanguageOption? _selectedLanguage;
    private string _saveMessage = string.Empty;

    public string DefaultCity
    {
        get => _defaultCity;
        set => SetProperty(ref _defaultCity, value);
    }

    public LanguageOption? SelectedLanguage
    {
        get => _selectedLanguage;
        set => SetProperty(ref _selectedLanguage, value);
    }

    public string SaveMessage
    {
        get => _saveMessage;
        set => SetProperty(ref _saveMessage, value);
    }

    public ObservableCollection<LanguageOption> Languages { get; } = new()
    {
        new() { Code = "fr", Label = "Français" },
        new() { Code = "en", Label = "English" },
        new() { Code = "de", Label = "Deutsch" },
        new() { Code = "es", Label = "Español" },
        new() { Code = "it", Label = "Italiano" },
        new() { Code = "pt", Label = "Português" },
        new() { Code = "nl", Label = "Nederlands" },
        new() { Code = "ru", Label = "Русский" },
        new() { Code = "zh_cn", Label = "中文 (简体)" },
        new() { Code = "ja", Label = "日本語" },
    };

    public SettingsViewModel(OptionsService optionsService)
    {
        _optionsService = optionsService;

        DefaultCity = optionsService.Options.DefaultCity;
        SelectedLanguage = Languages.FirstOrDefault(l => l.Code == optionsService.Options.Language)
                           ?? Languages[0];
    }

    public void Save()
    {
        _optionsService.Options.DefaultCity = DefaultCity;
        _optionsService.Options.Language = SelectedLanguage?.Code ?? "fr";
        _optionsService.Save();
        SaveMessage = "Paramètres enregistrés !";
    }
}