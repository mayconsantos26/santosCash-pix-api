using Models;

namespace Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user, string role); // Método para gerar um token JWT
    Task<RefreshToken> GenerateRefreshTokenAsync(); // Método para gerar um RefreshToken
}
