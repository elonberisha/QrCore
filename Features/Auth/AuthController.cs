using Microsoft.AspNetCore.Mvc;

namespace QrEventApi.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IConfiguration configuration, IJwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        var demoUsername = configuration["DemoUser:Username"];
        var demoPassword = configuration["DemoUser:Password"];

        if (!string.Equals(request.Username, demoUsername, StringComparison.Ordinal) ||
            !string.Equals(request.Password, demoPassword, StringComparison.Ordinal))
        {
            return Unauthorized(new { message = "Username ose password gabim." });
        }

        var (token, expiresAtUtc) = jwtTokenService.CreateToken(request.Username);
        return Ok(new LoginResponse
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc

        });
    }
}
