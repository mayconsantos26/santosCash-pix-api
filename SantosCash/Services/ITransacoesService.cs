using DTOs;

namespace Services;

public interface ITransacoesService
{
    Task<IEnumerable<TransacoesDTO>> GetAllAsync();
    Task<TransacoesDTO> GetTransacoesDTOByTxidAsync(string txid);
    Task<TransacoesCreateResponseDTO> CreateTransacoesDTOAsync(TransacoesCreateRequestDTO createDTO);
    Task<TransacoesDTO> UpdateTransacoesDTOAsync(TransacoesUpdateDTO transacoesUpdateDTO);
    Task<TransacoesDTO> DeleteTransacoesDTOAsync(string delete);
}
