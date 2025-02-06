using DTOs.User;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Services.Autentication;
public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO);
    Task<string> LoginAsync(LoginDTO loginDTO);
    Task<string> RefreshTokenAsync(TokenDTO refreshTokenDTO);
    Task<string> GenerateAccessToken(ApplicationUser user); 
}
