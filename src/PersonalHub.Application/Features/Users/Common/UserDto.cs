namespace PersonalHub.Application.Features.Users.Common;


using System.Text.Json.Serialization;

public class UserDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}