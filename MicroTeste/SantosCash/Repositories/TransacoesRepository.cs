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
        var transacaoExistente = await _context.Transacoes.FindAsync(transacoes.Id);
        if (transacaoExistente == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }

        // Atualiza os campos
        transacaoExistente.E2E_Id = transacoes.E2E_Id;
        transacaoExistente.Pagador_Nome = transacoes.Pagador_Nome;
        transacaoExistente.Pagador_Cpf = transacoes.Pagador_Cpf;
        transacaoExistente.Pagador_Banco = transacoes.Pagador_Banco;
        transacaoExistente.Pagador_Agencia = transacoes.Pagador_Agencia;
        transacaoExistente.Pagador_Conta = transacoes.Pagador_Conta;
        transacaoExistente.Recebedor_Nome = transacoes.Recebedor_Nome;
        transacaoExistente.Recebedor_Cpf = transacoes.Recebedor_Cpf;
        transacaoExistente.Recebedor_Banco = transacoes.Recebedor_Banco;
        transacaoExistente.Recebedor_Agencia = transacoes.Recebedor_Agencia;
        transacaoExistente.Recebedor_Conta = transacoes.Recebedor_Conta;

        _context.Transacoes.Update(transacaoExistente);
        await _context.SaveChangesAsync();

        return transacaoExistente;
    }

    //Delete
    public async Task<Transacoes> DeleteTransacoesAsync(string id)
    {
        var transacao = await _context.Transacoes.FirstOrDefaultAsync(t => t.Id == id); // Obtém a transação pelo ID de forma assíncrona
        if (transacao == null)
        {
            return null; // Retorna null se a transação não for encontrada
        }

        _context.Transacoes.Remove(transacao); // Remove a transação do contexto
        await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona

        return transacao; // Retorna a transação removida
    }
}
