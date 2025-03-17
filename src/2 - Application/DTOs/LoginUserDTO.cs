using System.Text.Json.Serialization;

namespace MyApiV8.Application.DTOs;

public class LoginUserDTO
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("pwd")]
    public string Password { get; set; }
}