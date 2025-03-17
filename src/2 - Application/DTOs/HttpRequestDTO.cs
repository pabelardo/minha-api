using System.Text.Json.Serialization;

namespace MyApiV8.Application.DTOs;

public class HttpRequestDTO
{
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("body")]
    public object? Body { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string>? Headers { get; set; }

    [JsonPropertyName("queryParams")]
    public Dictionary<string, string>? QueryParams { get; set; }
}