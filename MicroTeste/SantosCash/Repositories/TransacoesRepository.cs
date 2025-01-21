using Dbcontext;
using Microsoft.EntityFrameworkCore;
using MicroTeste.Models;

namespace Repositories;

public class TransacoesRepository : ITransacoesRepository
{
    private readonly AppDbContext _context;

    public TransacoesRepository(AppDbContext context)
    {
        _context = context;
    }

    // Create
    public async Task<Transacoes> CreateTransacoesAsync(Transacoes transacoes)
    {
        await _context.Transacoes.AddAsync(transacoes);
        await _context.SaveChangesAsync();
        return transacoes;
    }

    // Read All
    public async Task<IEnumerable<Transacoes>> GetAll()
    {
        return await _context.Transacoes.ToListAsync();
    }

    // Read by Txid
    public async Task<Transacoes> GetTransacoesByIdAsync(string txid)
    {
        return await _context.Transacoes.FirstOrDefaultAsync(t => t.Txid == txid);
    }

    // Update
    public async Task<Transacoes> UpdateTransacoesAsync(Transacoes transacoes)
    {
        var transacaoExistente = await _context.Transacoes.FindAsync(transacoes.Txid);
        if (transacaoExistente == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }

        _context.Entry(transacaoExistente).CurrentValues.SetValues(transacoes);
        await _context.SaveChangesAsync();
        return transacaoExistente;
    }

    // Delete
    public async Task<Transacoes> DeleteTransacoesAsync(string txid)
    {
        var transacao = await _context.Transacoes.FirstOrDefaultAsync(t => t.Txid == txid);
        if (transacao == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync();
        return transacao;
    }
}
