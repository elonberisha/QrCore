using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace QrEventApi.Features.Auth;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateToken(string username);
}

public sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public (string Token, DateTime ExpiresAtUtc) CreateToken(string username)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(jwtSection.GetValue<int>("ExpiresMinutes"));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }
}
