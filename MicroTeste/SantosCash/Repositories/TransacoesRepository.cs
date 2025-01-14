using Dbcontext;
using Microsoft.EntityFrameworkCore;
using MicroTeste.Models;

namespace SantosCash;

public class TransacoesRepository : ITransacoesRepository
{
    private readonly AppDbContext _context; // Contexto do banco de dados privado
    public TransacoesRepository(AppDbContext context)
    {
        _context = context;
    }

    //Crete
    public async Task<Transacoes> CreateTransacoesAsync(Transacoes transacoes)
    {
        await _context.Transacoes.AddAsync(transacoes); // Adiciona a transação ao contexto de forma assíncrona
        await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona
        return transacoes; // Retorna a transação criada
    }

    //Read
    public async Task<IEnumerable<Transacoes>> GetAll()
    {
        return await _context.Transacoes.ToListAsync(); // Obtém todas as transações de forma assíncrona
    }

    public async Task<Transacoes> GetTransacoesByIdAsync(string id)
    {
        return await _context.Transacoes.FirstOrDefaultAsync(t => t.Id == id); // Obtém a transação pelo ID de forma assíncrona
    }

    //Update
    public async Task<Transacoes> UpdateTransacoesAsync(Transacoes transacoes)
    {
        throw new NotImplementedException();
    }

    //Delete
    public async Task<Transacoes> DeleteTransacoesAsync(string id)
    {
        throw new NotImplementedException();
    }
}
