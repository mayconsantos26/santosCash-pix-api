using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ItransacoesServices _transacoesServices;

    public TransacoesController(ItransacoesServices transacoesServices)
    {
        _transacoesServices = transacoesServices;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransacoesDTO>>> GetAll()
    {
        var transacoesDTO = await _transacoesServices.GetAll();
        return Ok(transacoesDTO);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransacoesDTO>> GetTransacoes(string id)
    {
        var transacoesDTO = await _transacoesServices.GetTransacoesDTOByIdAsync(id);
        if (transacoesDTO == null)
        {
            return NotFound();
        }
        return Ok(transacoesDTO);
    }

    [HttpPost]
    public async Task<ActionResult<TransacoesDTO>> CreateTransacoes(TransacoesDTO transacoes)
    {
        await _transacoesServices.CreateTransacoesDTOAsync(transacoes);
        return CreatedAtAction(nameof(GetTransacoes), new { id = transacoes.Id }, transacoes);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransacoes(string id, TransacoesDTO transacoes)
    {
        if (id != transacoes.Id)
        {
            return BadRequest();
        }

        await _transacoesServices.UpdateTransacoesDTOAsync(transacoes);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransacoes(string id)
    {
        var transacoes = await _transacoesServices.GetTransacoesDTOByIdAsync(id);
        if (transacoes == null)
        {
            return NotFound();
        }

        await _transacoesServices.DeleteTransacoesDTOAsync(id);
        return NoContent();
    }
}