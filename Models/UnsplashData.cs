using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Models;

public class UnsplashResponse
{
    [JsonProperty("results")]
    public List<UnsplashPhoto> Results { get; set; } = new();
}

public class UnsplashPhoto
{
    [JsonProperty("urls")]
    public UnsplashUrls Urls { get; set; } = new();
}

public class UnsplashUrls
{
    [JsonProperty("regular")]
    public string Regular { get; set; } = string.Empty;
}