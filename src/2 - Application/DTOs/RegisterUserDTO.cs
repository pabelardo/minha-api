using System.Text.Json.Serialization;

namespace MyApiV8.Application.DTOs;

public class RegisterUserDTO
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("pwd")]
    public string Password { get; set; }

    [JsonPropertyName("confirm_pwd")]
    public string ConfirmPassword { get; set; }
}