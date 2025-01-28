using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Models;
using DTOs.User;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenServices _tokenServices; 
    // Serviços de token são geralmente usados para gerar, validar e gerenciar tokens de autenticação, como JWT (JSON Web Tokens).
    private readonly UserManager<ApplicationUser> _userManager; // O UserManager é uma classe do ASP.NET Identity que fornece funcionalidades para gerenciar usuários, como criação, atualização, exclusão e consulta de usuários. 
    private readonly RoleManager<IdentityRole> _roleManager; // O RoleManager é uma classe do ASP.NET Identity que fornece funcionalidades para gerenciar papéis (roles), como criação, atualização, exclusão e consulta de papéis.
    private readonly IConfiguration _configuration; // A IConfiguration é usada para acessar configurações da aplicação, como strings de conexão, chaves de API, e outras configurações definidas em arquivos de configuração (por exemplo, appsettings.json).
    private readonly ILogger<AuthController> _logger; // O ILogger é usado para registrar logs e mensagens de depuração, erros, informações e outros eventos importantes durante a execução da aplicação.

    public AuthController(ITokenServices tokenService,
                          UserManager<ApplicationUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          IConfiguration configuration,
                          ILogger<AuthController> logger)
    {
        _tokenServices = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Route("CriarPerfil")]
    public async Task<IActionResult> CreateRole(string nomePerfil)
    {
        var roleExist = await _roleManager.RoleExistsAsync(nomePerfil);

        if (!roleExist)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(nomePerfil));

            if (roleResult.Succeeded)
            {
                _logger.LogInformation(1, "Perfil adicionado com sucesso");
                return StatusCode(StatusCodes.Status200OK,
                        new ResponseUserDTO { Status = "Sucesso", Message = 
                        $"Perfil {nomePerfil} adicionado com sucesso" });
            }
            else
            {
                _logger.LogInformation(2, "Erro");
                return StatusCode(StatusCodes.Status400BadRequest,
                   new ResponseUserDTO { Status = "Erro", Message = 
                       $"Problema ao adicionar Perfil {nomePerfil}" });
            }
        }
        return StatusCode(StatusCodes.Status400BadRequest,
          new ResponseUserDTO { Status = "Erro", Message = "Perfil já existe." });
    }

    // [HttpPost]
    // [Route("AdicionarUsuarioAoPerfil")]
    // public async Task<IActionResult> AddUserToRole(string email, string nomePerfil)
    // {
    //     var user = await _userManager.FindByEmailAsync(email);

    //     if (user != null)
    //     {
    //         var result = await _userManager.AddToRoleAsync(user, nomePerfil);
    //         if(result.Succeeded)
    //         {
    //             _logger.LogInformation(1, $"Usuário {user.Email} adicionado ao papel {nomePerfil}");
    //             return StatusCode(StatusCodes.Status200OK,
    //                    new ResponseUserDTO { Status = "Sucesso", Message = 
    //                    $"Usuário {user.Email} adicionado ao papel {nomePerfil}" });
    //         }
    //         else
    //         {
    //             _logger.LogInformation(1, $"Erro: Não foi possível adicionar o usuário {user.Email} ao papel {nomePerfil}");
    //             return StatusCode(StatusCodes.Status400BadRequest, new ResponseUserDTO
    //             {
    //                 Status = "Erro",
    //                 Message =$"Erro: Não foi possível adicionar o usuário {user.Email} ao papel {nomePerfil}"
    //             });
    //         }
    //     }
    //     return BadRequest(new { error = "Não foi possível encontrar o usuário" });
    // }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenServices.GenerateAccessToken(authClaims,
                                                         _configuration);

            var refreshToken = _tokenServices.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                               out int refreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime =
                            DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("Registrar")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                   new ResponseUserDTO { Status = "Erro", Message = "Usuário já existe!" });
        }

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                   new ResponseUserDTO { Status = "Erro", Message = "Falha na criação do usuário." });
        }

        return Ok(new ResponseUserDTO { Status = "Sucesso", Message = "Usuário criado com sucesso!" });
    }

    [HttpPost]
    [Route("RevogarToken")]
    public async Task<IActionResult> RefreshToken(TokenDTO token)
    {
        if (token is null)
        {
            return BadRequest("Requisição inválida do cliente");
        }

        string? accessToken = token.AccessToken
                              ?? throw new ArgumentNullException(nameof(token));

        string? refreshToken = token.RefreshToken
                               ?? throw new ArgumentException(nameof(token));

        var principal = _tokenServices.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Token de acesso/refresh token inválido");
        }

        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken
                         || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Token de acesso/refresh token inválido");
        }

        var newAccessToken = _tokenServices.GenerateAccessToken(
                                           principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenServices.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost]
    [Route("Revogar/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return BadRequest("Nome de usuário inválido");

        user.RefreshToken = null;

        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}
