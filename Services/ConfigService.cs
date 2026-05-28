using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class ConfigService
{
    private const string ConfigPath = "config.json";

    public string ApiKey { get; private set; } = string.Empty;
    public string UnsplashApiKey { get; private set; } = string.Empty;

    public ConfigService()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(ConfigPath))
            throw new FileNotFoundException(
                $"Le fichier '{ConfigPath}' est introuvable. Créez-le à partir de 'config.example.json'.");

        var json = File.ReadAllText(ConfigPath);
        var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        if (config == null || !config.TryGetValue("ApiKey", out var key) || string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("La clé API météo (ApiKey) est manquante ou vide dans config.json.");
        
        ApiKey = key;

        if (!config.TryGetValue("UnsplashApiKey", out var unsplashKey) || string.IsNullOrWhiteSpace(unsplashKey))
            throw new InvalidOperationException("La clé API Unsplash (UnsplashApiKey) est manquante ou vide dans config.json.");
        
        UnsplashApiKey = unsplashKey;
    }
}