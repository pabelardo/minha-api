using System.Text.Json.Serialization;

namespace MyApiV8.Application.DTOs;

public class ClaimDTO
{
    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}