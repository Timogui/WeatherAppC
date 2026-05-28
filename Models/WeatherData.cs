using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Models;

public class WeatherData
{
    [JsonProperty("name")]
    public string CityName { get; set; } = string.Empty;

    [JsonProperty("coord")]
    public Coordinates Coord { get; set; } = new();

    [JsonProperty("main")]
    public MainData Main { get; set; } = new();

    [JsonProperty("weather")]
    public List<WeatherDescription> Weather { get; set; } = new();

    [JsonProperty("dt")]
    public long Timestamp { get; set; }
}

public class Coordinates
{
    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }
}

public class MainData
{
    [JsonProperty("temp")]
    public double Temp { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }
}

public class WeatherDescription
{
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("icon")]
    public string Icon { get; set; } = string.Empty;
}
