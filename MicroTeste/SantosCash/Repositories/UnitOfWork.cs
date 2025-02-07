using Data;

namespace Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private ITransacoesRepository _transacoesRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    // Exposição do contexto
    public AppDbContext Context => _context;

    // Repositório de transações
    public ITransacoesRepository TransacoesRepository
        => _transacoesRepository ??= new TransacoesRepository(this);

    // Salvar mudanças no banco
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Liberar recursos
    public void Dispose()
    {
        _context.Dispose();
    }
}
