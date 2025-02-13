using AutoMapper;
using DTOs;
using Helpers;
using Models;
using Repositories;

namespace Services;

public class TransacoesService : ITransacoesService
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IMapper _mapper;

    public TransacoesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransacoesDTO>> GetAllAsync()
    {
        var transacoes = await _unitOfWork.TransacoesRepository.GetAllAsync();
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<IEnumerable<TransacoesDTO>>(transacoes);
    }

    public async Task<TransacoesDTO> GetTransacoesDTOByTxidAsync(string txid)
    {
        var transacao = await _unitOfWork.TransacoesRepository.GetByTxidAsync(txid);
        if (transacao == null)
            throw new KeyNotFoundException("Transação não encontrada.");

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TransacoesDTO>(transacao);
    }

    public async Task<TransacoesCreateResponseDTO> CreateTransacoesDTOAsync(TransacoesCreateRequestDTO createDTO)
    {
        var transacao = _mapper.Map<Transacoes>(createDTO);
        transacao.Txid = PixHelpers.GerarTxid();
        transacao.Data_Transacao = DateTime.Now;

        var createdEntity = await _unitOfWork.TransacoesRepository.CreateAsync(transacao);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TransacoesCreateResponseDTO>(createdEntity);
    }

    public async Task<TransacoesDTO> UpdateTransacoesDTOAsync(TransacoesUpdateDTO updateDTO)
    {
        var transacao = await _unitOfWork.TransacoesRepository.GetByTxidAsync(updateDTO.Txid);
        if (transacao == null)
            throw new KeyNotFoundException("Transação não encontrada.");

        _mapper.Map(updateDTO, transacao);
        
        var updatedEntity = await _unitOfWork.TransacoesRepository.UpdateAsync(transacao); // Atualiza a transação

        updatedEntity.E2E_Id = PixHelpers.GerarEndToEndId(DateTime.Now); // Atualiza o E2E_Id

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TransacoesDTO>(updatedEntity);
    }

    public async Task<TransacoesDTO> DeleteTransacoesDTOAsync(string txid)
    {
        var transacao = await _unitOfWork.TransacoesRepository.DeleteByTxidAsync(txid);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TransacoesDTO>(transacao);
    }
}
