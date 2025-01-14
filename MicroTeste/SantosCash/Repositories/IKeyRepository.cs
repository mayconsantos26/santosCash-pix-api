using MicroTeste.Models;

namespace SantosCash;

public interface IKeyRepository
{
    Task<IEnumerable<Keys>> GetAllAsync();
}
