using MicroTeste.Models;

namespace Repositories;

public interface ITransacoesRepository
{
    Task<IEnumerable<Transacoes>> GetAll();
    Task<Transacoes> GetTransacoesByIdAsync(string id);
    Task<Transacoes> CreateTransacoesAsync(Transacoes transacoes);
    Task<Transacoes> UpdateTransacoesAsync(Transacoes transacoes);
    Task<Transacoes> DeleteTransacoesAsync(string id);
}

