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

    public async Task<IEnumerable<TransacoesDTO>> GetAll()
    {
        var transacoes = await _unitOfWork.TransacoesRepository.GetAll();
        return _mapper.Map<IEnumerable<TransacoesDTO>>(transacoes);
    }

    public async Task<TransacoesDTO> GetTransacoesDTOByTxidAsync(string txid)
    {
        var transacao = await _unitOfWork.TransacoesRepository.GetByTxidAsync(txid);
        if (transacao == null)
            throw new KeyNotFoundException("Transação não encontrada.");

        return _mapper.Map<TransacoesDTO>(transacao);
    }

    public async Task<TransacoesCreateResponseDTO> CreateTransacoesDTOAsync(TransacoesCreateRequestDTO request)
    {
        var transacao = _mapper.Map<Transacoes>(request);
        transacao.Txid = PixHelpers.GerarTxid();
        transacao.Data_Transacao = DateTime.Now;

        var createdEntity = await _unitOfWork.TransacoesRepository.CreateAsync(transacao);
        return _mapper.Map<TransacoesCreateResponseDTO>(createdEntity);
    }

    public async Task<TransacoesDTO> UpdateTransacoesDTOAsync(TransacoesUpdateDTO dto)
    {
        var transacao = await _unitOfWork.TransacoesRepository.GetByTxidAsync(dto.Txid);
        if (transacao == null)
            throw new KeyNotFoundException("Transação não encontrada.");

        _mapper.Map(dto, transacao);
        var updatedEntity = await _unitOfWork.TransacoesRepository.UpdateAsync(transacao);
        return _mapper.Map<TransacoesDTO>(updatedEntity);
    }

    public async Task<TransacoesDTO> DeleteTransacoesDTOAsync(string txid)
    {
        var transacao = await _unitOfWork.TransacoesRepository.DeleteByTxidAsync(txid);
        return _mapper.Map<TransacoesDTO>(transacao);
    }
}
