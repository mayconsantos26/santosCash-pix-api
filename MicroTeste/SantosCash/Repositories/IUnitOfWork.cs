using Data;

namespace Repositories;

public interface IUnitOfWork : IDisposable
{
    AppDbContext Context { get; } // Contexto do banco de dados
    ITransacoesRepository TransacoesRepository { get; } // Repositório de transações
    Task<int> SaveChangesAsync(); // Salvar mudanças no banco de dados
    public void Dispose(); // Liberar recursos
}
