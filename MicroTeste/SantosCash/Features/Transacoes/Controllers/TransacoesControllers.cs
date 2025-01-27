using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTOs;
using Services;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransacoesController : ControllerBase
{
    private readonly ITransacoesServices _transacoesServices;
    private readonly IConfiguration _configuration;

    public TransacoesController(ITransacoesServices transacoesServices, IConfiguration configuration)
    {
        _transacoesServices = transacoesServices;
        _configuration = configuration;
    }

    // Endpoint de Login
    // [HttpPost("login")]
    // [AllowAnonymous] // Permite acesso sem autenticação
    // public IActionResult Login([FromBody] LoginModel login)
    // {
    //     // Simulação de validação do usuário
    //     if (login.Username != "admin" || login.Password != "senha123")
    //     {
    //         return Unauthorized(new { message = "Usuário ou senha inválidos." });
    //     }

    //     // Gerar o token JWT
    //     var jwtSettings = _configuration.GetSection("JwtSettings");
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //     var claims = new[]
    //     {
    //         new Claim(JwtRegisteredClaimNames.Sub, login.Username),
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //     };

    //     var token = new JwtSecurityToken(
    //         issuer: jwtSettings["Issuer"],
    //         audience: jwtSettings["Audience"],
    //         claims: claims,
    //         expires: DateTime.UtcNow.AddHours(1),
    //         signingCredentials: creds
    //     );

    //     return Ok(new
    //     {
    //         Token = new JwtSecurityTokenHandler().WriteToken(token),
    //         Expiration = token.ValidTo
    //     });
    // }

    // GET: api/Transacoes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransacoesDTO>>> GetAllTransacoes()
    {
        try
        {
            var transacoes = await _transacoesServices.GetAll();
            return Ok(transacoes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // GET: api/Transacoes/{txid}
    [HttpGet("{txid}")]
    public async Task<ActionResult<TransacoesDTO>> GetTransacaoByTxid(string txid)
    {
        try
        {
            var transacao = await _transacoesServices.GetTransacoesDTOByTxidAsync(txid);
            if (transacao == null)
            {
                return NotFound(new { message = "Transação não encontrada." });
            }
            return Ok(transacao);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Transação não encontrada." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // POST: api/Transacoes
    [HttpPost]
    // [Authorize] // Protegido por autenticação
    public async Task<ActionResult<TransacoesCreateResponseDTO>> CreateTransacao([FromBody] TransacoesCreateRequestDTO request)
    {
        try
        {
            var createdTransacao = await _transacoesServices.CreateTransacoesDTOAsync(request);
            return CreatedAtAction(nameof(GetTransacaoByTxid), new { txid = createdTransacao.Txid }, createdTransacao);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // PUT: api/Transacoes/{txid}
    [HttpPut("{txid}")]
    // [Authorize] // Protegido por autenticação
    public async Task<ActionResult<TransacoesDTO>> UpdateTransacao(string txid, [FromBody] TransacoesUpdateDTO request)
    {
        try
        {
            if (txid != request.Txid)
            {
                return BadRequest(new { message = "Txid no corpo e na URL não correspondem." });
            }

            var updatedTransacao = await _transacoesServices.UpdateTransacoesDTOAsync(request);
            return Ok(updatedTransacao);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Transação não encontrada." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // DELETE: api/Transacoes/{txid}
    [HttpDelete("{txid}")]
    // [Authorize] // Protegido por autenticação
    public async Task<ActionResult<TransacoesDTO>> DeleteTransacao(string txid)
    {
        try
        {
            var deletedTransacao = await _transacoesServices.DeleteTransacoesDTOAsync(txid);
            return Ok(deletedTransacao);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Transação não encontrada." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

// Modelo de Login
public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
