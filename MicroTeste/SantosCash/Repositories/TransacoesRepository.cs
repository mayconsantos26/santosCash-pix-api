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
    public async Task<IEnumerable<Transacoes>> GetAllAsync()
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
    public async Task<Transacoes> CreateAsync(Transacoes create)
    {
        if (create == null)
            throw new ArgumentNullException(nameof(create));

        _unitOfWork.Context.Transacoes.Add(create);
        return create;
    }

    // Atualizar transação existente
    public async Task<Transacoes> UpdateAsync(Transacoes update)
    {
        if (update == null)
            throw new ArgumentNullException(nameof(update));

        _unitOfWork.Context.Transacoes.Update(update);
        return update;
    }

    // Remover transação por Txid
    public async Task<Transacoes> DeleteByTxidAsync(string delete)
    {
        var transacao = await GetByTxidAsync(delete);
        if (transacao == null)
            throw new KeyNotFoundException("Transação não encontrada.");

        _unitOfWork.Context.Transacoes.Remove(transacao);
        return transacao;
    }
}