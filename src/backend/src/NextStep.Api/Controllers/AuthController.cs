using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NextStep.Api.Auth;

namespace NextStep.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthSettings _authSettings;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IOptions<AuthSettings> authSettings, IJwtTokenService jwtTokenService)
    {
        _authSettings = authSettings.Value;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        if (request.Email != _authSettings.Email || request.Password != _authSettings.Password)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        var token = _jwtTokenService.GenerateToken(request.Email);
        return Ok(new LoginResponse(token));
    }
}
