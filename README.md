# Weather App

Application météo en C# .NET avec Avalonia UI et l'API OpenWeatherMap

## Fonctionnalités

- **Onglet Recherche** : affiche la météo actuelle d'une ville
- **Onglet Prévisions** : affiche les prévisions sur 5 jours à 12h00
- **Onglet Paramètres** : permet de définir une ville par défaut et une langue pour les réponses de l'API

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- clé API [OpenWeatherMap](https://openweathermap.org/appid)

## Installation

1. Cloner le dépôt :
   ```bash
   git clone <url-du-repo>
   cd WeatherApp
   ```

2. Créer le fichier `config.json` à la racine, similaire à `config.example.json` :
   ```json
   {
     "ApiKey": "VOTRE_CLE_API_ICI"
   }
   ```

3. Installer les dépendances :
   ```bash
   dotnet restore
   ```

4. Lancer l'application :
   ```bash
   dotnet run
   ```

## Configuration

| Fichier | Rôle |
|---|---|
| `config.json` | Clé API OpenWeatherMap |
| `options.json` | Paramètres utilisateur (ville par défaut, langue) |

> Ces deux fichiers sont dans le `.gitignore` et ne doivent **jamais** être commités.

Le fichier `options.json` est créé automatiquement au premier lancement.

## Structure du projet

```
WeatherApp/
├── Models/
│   ├── AppOptions.cs
│   ├── ForecastData.cs
│   ├── UnsplashData.cs
│   └── WeatherData.cs
│
├── Services/
│   ├── ConfigService.cs
│   ├── OptionsService.cs
│   └── WeatherService.cs
│
├── ViewModels/
│   ├── ViewModelBase.cs
│   ├── SearchViewModel.cs
│   ├── ForecastViewModel.cs
│   ├── ForecastDayViewModel.cs
│   ├── SettingsViewModel.cs
│   └── MainViewModel.cs
│
├── Views/
│   ├── MainWindow.axaml
│   └── MainWindow.axaml.cs
│
├── App.axaml
├── AApp.axaml.cs
├── Program.cs
├── config.json
├── options.json
└── .gitignore
```