using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories;

public class TransacoesRepository : ITransacoesRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public TransacoesRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Obter todas as transações
    public async Task<IEnumerable<Transacoes>> GetAll()
    {
        return await _unitOfWork.Context.Transacoes.ToListAsync();
    }

    // Obter transação por Txid
    public async Task<Transacoes> GetByTxidAsync(string txid)
    {
        if (string.IsNullOrEmpty(txid))
            throw new ArgumentNullException(nameof(txid));

        return await _unitOfWork.Context.Transacoes
            .FirstOrDefaultAsync(t => t.Txid == txid);
    }

    // Criar uma nova transação
    public async Task<Transacoes> CreateAsync(Transacoes transacao)
    {
        if (transacao == null)
            throw new ArgumentNullException(nameof(transacao));

        _unitOfWork.Context.Transacoes.Add(transacao);
        await _unitOfWork.SaveChangesAsync();
        return transacao;
    }

    // Atualizar transação existente
    public async Task<Transacoes> UpdateAsync(Transacoes transacao)
    {
        if (transacao == null)
            throw new ArgumentNullException(nameof(transacao));

        _unitOfWork.Context.Transacoes.Update(transacao);
        await _unitOfWork.SaveChangesAsync();
        return transacao;
    }

    // Remover transação por Txid
    public async Task<Transacoes> DeleteByTxidAsync(string txid)
    {
        var transacao = await GetByTxidAsync(txid);
        if (transacao == null)
            throw new KeyNotFoundException("Transação não encontrada.");

        _unitOfWork.Context.Transacoes.Remove(transacao);
        await _unitOfWork.SaveChangesAsync();
        return transacao;
    }
}