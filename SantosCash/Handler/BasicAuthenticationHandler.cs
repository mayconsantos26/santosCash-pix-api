using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Handler;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Extraímos o cabeçalho de autenticação
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        var authorizationHeader = Request.Headers["Authorization"].ToString();
        if (authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            var encodedCredentials = authorizationHeader.Substring(6).Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials)).Split(":");

            var username = credentials[0];
            var password = credentials[1];

            // Validação simples de credenciais (substitua por lógica real)
            if (username == "Admin" && password == "Admin@@123")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username)
                };
                var identity = new ClaimsIdentity(claims, "Basic");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "BasicAuthentication");

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}
