using Microsoft.AspNetCore.Mvc;
using DTOs;
using Services;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransacoesController : ControllerBase
{
    private readonly ITransacoesServices _transacoesServices;

    public TransacoesController(ITransacoesServices transacoesServices)
    {
        _transacoesServices = transacoesServices;
    }

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
    public async Task<ActionResult<TransacoesCreateResponseDTO>> CreateTransacao(TransacoesCreateRequestDTO request)
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
    public async Task<ActionResult<TransacoesDTO>> UpdateTransacao(string txid, TransacoesUpdateDTO request)
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
