using Models;

namespace Interfaces;

public interface ITransacoesRepository
{
    Task<IEnumerable<Transacoes>> GetAll();
    Task<Transacoes> GetByTxidAsync(string txid);
    Task<Transacoes> CreateAsync(Transacoes transacoes);
    Task<Transacoes> UpdateAsync(Transacoes transacoes);
    Task<Transacoes> DeleteByTxidAsync(string txid);
}

