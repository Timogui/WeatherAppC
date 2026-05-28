using System.IO;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class OptionsService
{
    private const string OptionsPath = "options.json";

    public AppOptions Options { get; private set; } = new();

    public OptionsService()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(OptionsPath))
        {
            Options = new AppOptions();
            Save();
            return;
        }

        var json = File.ReadAllText(OptionsPath);
        Options = JsonConvert.DeserializeObject<AppOptions>(json) ?? new AppOptions();
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(Options, Formatting.Indented);
        File.WriteAllText(OptionsPath, json);
    }
}