using DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Services.Autentication;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var result = await _authService.RegisterAsync(registerDTO);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { Message = "Usuário registrado com sucesso!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        try
        {
            var token = await _authService.LoginAsync(loginDTO);
            return Ok(new { AccessToken = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = "Acesso não autorizado: " + ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDTO refreshTokenDTO)
    {
        try
        {
            var token = await _authService.RefreshTokenAsync(refreshTokenDTO);
            return Ok(new { AccessToken = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = "Token inválido ou expirado: " + ex.Message });
        }
    }
}
