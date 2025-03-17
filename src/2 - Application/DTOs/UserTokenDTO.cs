using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace MyApiV8.Application.DTOs;

public class UserTokenDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("claims")]
    public IEnumerable<ClaimDTO> Claims { get; set; }

    [JsonPropertyName("roles")]
    public List<IdentityRole<Guid>> Roles { get; set; }

    public UserTokenDTO()
    {
        Claims = [];
        Roles = [];
    }
}