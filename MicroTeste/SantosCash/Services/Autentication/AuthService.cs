using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data;
using DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Services.Autentication;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO)
    {
        var user = new ApplicationUser
        {
            UserName = registerDTO.Username,
            Email = registerDTO.Email
        };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        if (!string.IsNullOrEmpty(registerDTO.Role))
        {
            if (!await _roleManager.RoleExistsAsync(registerDTO.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerDTO.Role));
            }

            await _userManager.AddToRoleAsync(user, registerDTO.Role);
        }

        return result;
    }

    public async Task<string> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByNameAsync(loginDTO.Username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var accessToken = await GenerateAccessToken(user);  // Agora assíncrono
        var refreshToken = Guid.NewGuid().ToString();

        // Armazenar o refresh token no banco de dados do usuário
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Expiração do refresh token (7 dias por exemplo)
        await _userManager.UpdateAsync(user); // Atualiza o usuário com o refresh token

        return accessToken;  // Retorna o access token
    }

    public async Task<string> RefreshTokenAsync(TokenDTO refreshTokenDTO)
    {
        // Lógica para validar o refresh token
        var user = await _userManager.FindByNameAsync(refreshTokenDTO.Username);
        if (user == null || user.RefreshToken != refreshTokenDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new UnauthorizedAccessException("Token de atualização inválido");
        }

        var newAccessToken = await GenerateAccessToken(user);
        var newRefreshToken = Guid.NewGuid().ToString();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        return newAccessToken;
    }

    public async Task<string> GenerateAccessToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}