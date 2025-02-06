using DTOs.User;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        // Verifica se o modelo é válido
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized("Credenciais inválidas.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = await _tokenService.GenerateTokenAsync(user, roles.FirstOrDefault() ?? "User");

        var refreshToken = await _tokenService.GenerateRefreshTokenAsync();
        
        // Atualiza o refresh token e a data de expiração do usuário
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpiryTime = refreshToken.Expiration;
        await _userManager.UpdateAsync(user);

        // Retorna apenas token e refreshToken no JSON
        return Ok(new 
        { 
            token = token,
            refreshToken = refreshToken.Token
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO model)
    {
        if (!ModelState.IsValid) // Verifica se o modelo é válido
        {
            // Retorna as mensagens de erro de validação
            return BadRequest(ModelState);
        }

        var user = new ApplicationUser { UserName = model.Username }; // Cria um novo usuário
        var result = await _userManager.CreateAsync(user, model.Password); // Registra o usuário

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (!await _roleManager.RoleExistsAsync(model.Role))
            await _roleManager.CreateAsync(new IdentityRole(model.Role));

        await _userManager.AddToRoleAsync(user, model.Role);

        return Ok("Usuário registrado com sucesso.");
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var user = await _userManager
            .Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

        if (user == null)
        {
            return Unauthorized("Refresh token inválido ou expirado.");
        }

        // Geração de novo access token
        var roles = await _userManager.GetRolesAsync(user);
        var newToken = await _tokenService.GenerateTokenAsync(user, roles.FirstOrDefault() ?? "User");

        // Gerar novo refresh token
        var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();
        
        // Atualizar o usuário com o novo refresh token e sua expiração
        user.RefreshToken = newRefreshToken.Token;
        user.RefreshTokenExpiryTime = newRefreshToken.Expiration;
        await _userManager.UpdateAsync(user);

        // Retornar apenas token e refreshToken
        return Ok(new 
        { 
            token = newToken, 
            refreshToken = newRefreshToken.Token 
        });
    }

}