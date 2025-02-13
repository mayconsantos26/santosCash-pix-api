using Models;

namespace Repositories;

public interface ITransacoesRepository
{
    Task<IEnumerable<Transacoes>> GetAllAsync();
    Task<Transacoes> GetByTxidAsync(string txid);
    Task<Transacoes> CreateAsync(Transacoes create);
    Task<Transacoes> UpdateAsync(Transacoes update);
    Task<Transacoes> DeleteByTxidAsync(string delete);
}

