using AutoMapper;
using DTOs;
using Repositories;
using Helpers;

namespace Services;

public class TransacoesServices : ITransacoesServices
{
    private readonly ITransacoesRepository _transacoesRepository;
    private readonly IMapper _mapper;

    public TransacoesServices(ITransacoesRepository transacoesRepository, IMapper mapper)
    {
        _transacoesRepository = transacoesRepository;
        _mapper = mapper;
    }

    // Create
    public async Task<TransacoesCreateResponseDTO> CreateTransacoesDTOAsync(TransacoesCreateRequestDTO request)
    {
        if (request.Valor <= 0)
        {
            throw new ArgumentException("O valor da transação deve ser maior que zero.");
        }

        // Gerar E2E ID (usando seu helper PixHelpers)
        var e2eId = PixHelpers.GerarEndToEndId(DateTime.Now);

        var transacoesCreateDTO = _mapper.Map<MicroTeste.Models.Transacoes>(request);
        transacoesCreateDTO.E2E_Id = e2eId;  // Atribui o ID gerado ao DTO

        var transacaoEntity = _mapper.Map<MicroTeste.Models.Transacoes>(transacoesCreateDTO);
        var createdEntity = await _transacoesRepository.CreateTransacoesAsync(transacaoEntity);
        return _mapper.Map<TransacoesCreateResponseDTO>(createdEntity);
    }

    // Read All
    public async Task<IEnumerable<TransacoesDTO>> GetAll()
    {
        var transacoesEntity = await _transacoesRepository.GetAll();
        return _mapper.Map<IEnumerable<TransacoesDTO>>(transacoesEntity);
    }

    // Read Txid
    // Read Txid
    public async Task<TransacoesDTO> GetTransacoesDTOByTxidAsync(string txid)
    {
        var transacaoEntity = await _transacoesRepository.GetTransacoesByIdAsync(txid); // Corrige o nome do método no repositório
        if (transacaoEntity == null)
        {
            throw new KeyNotFoundException("Transação não encontrada."); // Mensagem de erro permanece a mesma
        }
        return _mapper.Map<TransacoesDTO>(transacaoEntity); // Retorna o mapeamento do resultado
    }


    // Update
    public async Task<TransacoesUpdateDTO> UpdateTransacoesDTOAsync(TransacoesUpdateDTO transacoesUpdateDTO)
    {
        var transacaoEntity = _mapper.Map<MicroTeste.Models.Transacoes>(transacoesUpdateDTO);
        var updatedEntity = await _transacoesRepository.UpdateTransacoesAsync(transacaoEntity);
        return _mapper.Map<TransacoesUpdateDTO>(updatedEntity);
    }

    // Delete
    public async Task<TransacoesDTO> DeleteTransacoesDTOAsync(string txid)
    {
        var transacaoEntity = await _transacoesRepository.DeleteTransacoesAsync(txid);
        if (transacaoEntity == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }
        return _mapper.Map<TransacoesDTO>(transacaoEntity);
    }
}
