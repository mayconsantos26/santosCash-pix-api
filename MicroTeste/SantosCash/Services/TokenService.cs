using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Interfaces;
using Models;

namespace Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Geração assíncrona do token de acesso
        public async Task<string> GenerateTokenAsync(ApplicationUser user, string role)
        {
            var claims = new[] // Dados que serão armazenados no token
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // Nome do usuário cadastrado
                new Claim(ClaimTypes.Role, role) // Permissões do usuário
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])); // Chave de segurança configurada no appsettings.json
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Credenciais de segurança

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddHours(1), // Tempo de expiração do token
                claims: claims, // Dados do usuário
                signingCredentials: creds); // Credenciais de segurança

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token)); // Retorna o token gerado de maneira assíncrona
        }

        // Geração assíncrona do refresh token
        public async Task<RefreshToken> GenerateRefreshTokenAsync()
        {
            return await Task.FromResult(new RefreshToken
            {
                Token = Guid.NewGuid().ToString(), // Gera um novo refresh token
                Expiration = DateTime.UtcNow.AddDays(7) // Expira em 7 dias
            });
        }
    }
}
