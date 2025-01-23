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

    // Read All
    public async Task<IEnumerable<TransacoesDTO>> GetAll()
    {
        var transacoesEntity = await _transacoesRepository.GetAll();
        return _mapper.Map<IEnumerable<TransacoesDTO>>(transacoesEntity);
    }

    // Read By Txid
    public async Task<TransacoesDTO> GetTransacoesDTOByTxidAsync(string txid)
    {
        var transacaoEntity = await _transacoesRepository.GetByTxidAsync(txid);
        if (transacaoEntity == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }

        return _mapper.Map<TransacoesDTO>(transacaoEntity);
    }

    // Create
    public async Task<TransacoesCreateResponseDTO> CreateTransacoesDTOAsync(TransacoesCreateRequestDTO request)
    {
        Transacoes transacao = _mapper.Map<Transacoes>(request);
        transacao.Txid = PixHelpers.GerarTxid();
        transacao.Data_Transacao = DateTime.Now;

        var createdEntity = await _transacoesRepository.CreateAsync(transacao);
            var returnEntity = _mapper.Map<TransacoesCreateResponseDTO>(createdEntity);
            return returnEntity;
    }

    // Update
    public async Task<TransacoesDTO> UpdateTransacoesDTOAsync(TransacoesUpdateDTO transacoesUpdateDTO)
    {
        var transacaoExistente = await _transacoesRepository.GetByTxidAsync(transacoesUpdateDTO.Txid);
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
        var updatedEntity = await _transacoesRepository.UpdateAsync(transacaoAtualizada);
        return _mapper.Map<TransacoesDTO>(updatedEntity);
    }

    // Delete
    public async Task<TransacoesDTO> DeleteTransacoesDTOAsync(string txid)
    {
        var transacaoExistente = await _transacoesRepository.GetByTxidAsync(txid);
        if (transacaoExistente == null)
        {
            throw new KeyNotFoundException("Transação não encontrada.");
        }

        var transacaoRemovida = await _transacoesRepository.DeleteByTxidAsync(txid);
        return _mapper.Map<TransacoesDTO>(transacaoRemovida);
    }
}
