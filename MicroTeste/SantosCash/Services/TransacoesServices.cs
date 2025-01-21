using AutoMapper;
using DTOs;
using Repositories;
using Helpers;
using MicroTeste.Models;

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
        Transacoes transacao = _mapper.Map<Transacoes>(request);
        transacao.Txid = PixHelpers.GerarTxid();
        transacao.Data_Transacao = DateTime.Now;

        var createdEntity = await _transacoesRepository.CreateTransacoesAsync(transacao);
            var returnEntity = _mapper.Map<TransacoesCreateResponseDTO>(createdEntity);
            return returnEntity;
    }

    // Read All
    public async Task<IEnumerable<TransacoesDTO>> GetAll()
    {
        var transacoesEntity = await _transacoesRepository.GetAll();
        return _mapper.Map<IEnumerable<TransacoesDTO>>(transacoesEntity);
    }

    // Read By Txid
    public async Task<TransacoesDTO> GetTransacoesDTOByTxidAsync(string txid)
    {
        var transacaoEntity = await _transacoesRepository.GetTransacoesByTxidAsync(txid);
        if (transacaoEntity == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }

        return _mapper.Map<TransacoesDTO>(transacaoEntity);
    }

    // Update
    public async Task<TransacoesUpdateDTO> UpdateTransacoesDTOAsync(TransacoesUpdateDTO transacoesUpdateDTO)
    {
        var transacaoExistente = await _transacoesRepository.GetTransacoesByTxidAsync(transacoesUpdateDTO.Txid);
        if (transacaoExistente == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }

        if (!CpfValidator.IsValidCpf(transacoesUpdateDTO.Pagador_Cpf) ||
            !CpfValidator.IsValidCpf(transacoesUpdateDTO.Recebedor_Cpf))
        {
            throw new ArgumentException("Um ou mais CPFs fornecidos são inválidos.");
        }

        var transacaoAtualizada = _mapper.Map(transacoesUpdateDTO, transacaoExistente);
        var updatedEntity = await _transacoesRepository.UpdateTransacoesAsync(transacaoAtualizada);
        return _mapper.Map<TransacoesUpdateDTO>(updatedEntity);
    }

    // Delete
    public async Task<TransacoesDTO> DeleteTransacoesDTOAsync(string txid)
    {
        try
        {
            var transacaoRemovida = await _transacoesRepository.DeleteTransacoesAsync(txid);
            return _mapper.Map<TransacoesDTO>(transacaoRemovida);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
    }
}
