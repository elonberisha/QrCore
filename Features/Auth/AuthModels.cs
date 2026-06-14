using System.ComponentModel.DataAnnotations;

namespace QrEventApi.Features.Auth;

public sealed class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public sealed class LoginResponse
{
    public string TokenType { get; set; } = "Bearer";
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
}
