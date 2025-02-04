using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Models;
using DTOs.User;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenServices _tokenServices;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenServices tokenServices,
                          UserManager<ApplicationUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          IConfiguration configuration,
                          ILogger<AuthController> logger)
    {
        _tokenServices = tokenServices;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    /// Cria um novo perfil (role) no sistema.
    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            return BadRequest(new ResponseUserDTO { Status = "Error", Message = "Role already exists." });
        }

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
        {
            _logger.LogInformation($"Role '{roleName}' created successfully.");
            return Ok(new ResponseUserDTO { Status = "Success", Message = $"Role '{roleName}' created successfully." });
        }

        _logger.LogError($"Error creating role '{roleName}'.");
        return BadRequest(new ResponseUserDTO { Status = "Error", Message = $"Failed to create role '{roleName}'." });
    }

    /// Registra um novo usuário no sistema.
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (await _userManager.FindByNameAsync(model.Username!) != null)
        {
            return BadRequest(new ResponseUserDTO { Status = "Error", Message = "User already exists." });
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
        {
            return StatusCode(500, new ResponseUserDTO { Status = "Error", Message = "User creation failed." });
        }

        return Ok(new ResponseUserDTO { Status = "Success", Message = "User created successfully." });
    }

    /// Autentica o usuário e retorna tokens de acesso e refresh.
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            return Unauthorized(new { Error = "Invalid username or password." });
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = _tokenServices.GenerateAccessToken(authClaims, _configuration);
        var refreshToken = _tokenServices.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:RefreshTokenValidityInMinutes"));

        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }

    /// Atualiza o token de acesso usando o refresh token.
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(TokenDTO token)
    {
        if (token == null)
        {
            return BadRequest("Invalid client request.");
        }

        var principal = _tokenServices.GetPrincipalFromExpiredToken(token.AccessToken!, _configuration);
        var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);

        if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access or refresh token.");
        }

        var newAccessToken = _tokenServices.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenServices.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        });
    }

    /// Revoga o token de um usuário específico.
    [Authorize]
    [HttpPost("Revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound(new ResponseUserDTO { Status = "Error", Message = "User not found." });
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            _logger.LogInformation($"Refresh token revoked for user '{username}'.");
            return Ok(new ResponseUserDTO { Status = "Success", Message = $"Refresh token revoked for user '{username}'." });
        }

        _logger.LogError($"Error revoking refresh token for user '{username}'.");
        return StatusCode(500, new ResponseUserDTO { Status = "Error", Message = $"Failed to revoke refresh token for user '{username}'." });
    }
}