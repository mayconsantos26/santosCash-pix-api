using DTOs;

namespace Services;

public interface ITransacoesServices
{
    Task<IEnumerable<TransacoesDTO>> GetAll();
    Task<TransacoesDTO> GetTransacoesDTOByTxidAsync(string txid);
    Task<TransacoesCreateResponseDTO> CreateTransacoesDTOAsync(TransacoesCreateRequestDTO request);
    Task<TransacoesDTO> UpdateTransacoesDTOAsync(TransacoesUpdateDTO transacoesUpdateDTO);
    Task<TransacoesDTO> DeleteTransacoesDTOAsync(string txid);
}
