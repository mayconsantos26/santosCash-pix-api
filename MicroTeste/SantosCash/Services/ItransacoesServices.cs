using DTOs;
using MicroTeste.Models;

namespace Services;

public interface ItransacoesServices
{
    Task<IEnumerable<TransacoesDTO>> GetAll();
    Task<TransacoesDTO> GetTransacoesDTOByIdAsync(string id);
    Task<TransacoesDTO> CreateTransacoesDTOAsync(TransacoesDTO transacoesDTO);
    Task<TransacoesDTO> UpdateTransacoesDTOAsync(TransacoesDTO transacoesDTO);
    Task<TransacoesDTO> DeleteTransacoesDTOAsync(string id);
}
