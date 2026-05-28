using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Models;

public class ForecastResponse
{
    [JsonProperty("list")]
    public List<ForecastItem> List { get; set; } = new();

    [JsonProperty("city")]
    public ForecastCity City { get; set; } = new();
}

public class ForecastItem
{
    [JsonProperty("dt")]
    public long Timestamp { get; set; }

    [JsonProperty("dt_txt")]
    public string DateText { get; set; } = string.Empty;

    [JsonProperty("main")]
    public MainData Main { get; set; } = new();

    [JsonProperty("weather")]
    public List<WeatherDescription> Weather { get; set; } = new();

    public DateTime Date => DateTimeOffset.FromUnixTimeSeconds(Timestamp).LocalDateTime;
}

public class ForecastCity
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("coord")]
    public Coordinates Coord { get; set; } = new();
}
