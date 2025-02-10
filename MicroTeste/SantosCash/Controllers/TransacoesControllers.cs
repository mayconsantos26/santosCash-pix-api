using Microsoft.AspNetCore.Mvc;
using DTOs;
using Services;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransacoesController : ControllerBase
{
    private readonly ITransacoesService _transacoesService;

    public TransacoesController(ITransacoesService transacoesService)
    {
        _transacoesService = transacoesService;
    }

    // GET: api/Transacoes (Apenas Admin e Usuário podem acessar)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransacoesDTO>>> GetAllTransacoes()
    {
        try
        {
            var transacoes = await _transacoesService.GetAll();
            return Ok(transacoes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // GET: api/Transacoes/{txid} (Apenas Admin e Usuário podem acessar)
    [HttpGet("{txid}")]
    public async Task<ActionResult<TransacoesDTO>> GetTransacaoByTxid(string txid)
    {
        try
        {
            var transacao = await _transacoesService.GetTransacoesDTOByTxidAsync(txid);
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

    // POST: api/Transacoes (Apenas Admin e Usuário podem criar)
    [HttpPost]
    public async Task<ActionResult<TransacoesCreateResponseDTO>> CreateTransacao(TransacoesCreateRequestDTO request)
    {
        try
        {
            var createdTransacao = await _transacoesService.CreateTransacoesDTOAsync(request);
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

    // PUT: api/Transacoes/{txid} (Apenas Admin pode atualizar)
    [HttpPut("{txid}")]
    public async Task<ActionResult<TransacoesDTO>> UpdateTransacao(string txid, TransacoesUpdateDTO request)
    {
        try
        {
            if (txid != request.Txid)
            {
                return BadRequest(new { message = "Txid no corpo e na URL não correspondem." });
            }

            var updatedTransacao = await _transacoesService.UpdateTransacoesDTOAsync(request);
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

    // DELETE: api/Transacoes/{txid} (Apenas Admin pode deletar)
    [HttpDelete("{txid}")]
    public async Task<ActionResult<TransacoesDTO>> DeleteTransacao(string txid)
    {
        try
        {
            var deletedTransacao = await _transacoesService.DeleteTransacoesDTOAsync(txid);
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
