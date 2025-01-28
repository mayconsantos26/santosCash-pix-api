using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories;

public class TransacoesRepository : ITransacoesRepository
{
    private readonly AppDbContext _context;

    public TransacoesRepository(AppDbContext context)
    {
        _context = context;
    }

    // Read All
    public async Task<IEnumerable<Transacoes>> GetAll()
    {
        return await _context.Transacoes.ToListAsync();
    }

    // Read By Txid
    public async Task<Transacoes> GetByTxidAsync(string txid)
    {
        if (string.IsNullOrEmpty(txid))
        {
            throw new ArgumentNullException(nameof(txid));
        }

        return await _context.Transacoes.FirstOrDefaultAsync(t => t.Txid == txid);
    }

    // Create
    public async Task<Transacoes> CreateAsync(Transacoes transacoes)
    {
        if (transacoes == null)
        {
            throw new ArgumentNullException(nameof(transacoes));
        }

        var existingTransacao = await _context.Transacoes.FirstOrDefaultAsync(t => t.Txid == transacoes.Txid);
        if (existingTransacao != null)
        {
            throw new InvalidOperationException("Transação já existe.");
        }

        _context.Transacoes.Add(transacoes);
        await _context.SaveChangesAsync();
        return transacoes;
    }

    // Update
    public async Task<Transacoes> UpdateAsync(Transacoes transacoes)
    {
        if (transacoes == null)
        {
            throw new ArgumentNullException(nameof(transacoes));
        }

        var existingTransacao = await _context.Transacoes.FirstOrDefaultAsync(t => t.Txid == transacoes.Txid);
        if (existingTransacao == null)
        {
            throw new InvalidOperationException("Transação não encontrada.");
        }

        _context.Entry(existingTransacao).CurrentValues.SetValues(transacoes);
        await _context.SaveChangesAsync();
        return existingTransacao;
    }

    // Delete
    public async Task<Transacoes> DeleteByTxidAsync(string txid)
    {
        if (string.IsNullOrEmpty(txid))
        {
            throw new ArgumentNullException(nameof(txid));
        }

        var transacao = await _context.Transacoes.FirstOrDefaultAsync(t => t.Txid == txid);
        if (transacao == null)
        {
            throw new InvalidOperationException("Transação não encontrada.");
        }

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync();
        return transacao;
    }
}
