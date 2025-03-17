using System.Text.Json.Serialization;

namespace MyApiV8.Application.DTOs;

public class LoginResponseDTO
{
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public double ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("rt_expires_in")]
    public double RefreshTokenExpiresIn { get; set; }

    [JsonPropertyName("user_token")]
    public UserTokenDTO UserToken { get; set; }
}