using AutoMapper;
using DTOs;
using MicroTeste.Models;
using Repositories;

namespace Services;

public class TransacoesServices : ItransacoesServices
{
    private readonly ITransacoesRepository _transacoesRepository;
    private readonly IMapper _mapper;

    public TransacoesServices(ITransacoesRepository transacoesRepository, IMapper mapper)
    {
        _transacoesRepository = transacoesRepository;
        _mapper = mapper;
    }
    public async Task<TransacoesDTO> CreateTransacoesDTOAsync(TransacoesDTO transacoesDTO)
    {
        var transacoesEntity = _mapper.Map<Transacoes>(transacoesDTO);
        var createdEntity = await _transacoesRepository.CreateTransacoesAsync(transacoesEntity);
        return _mapper.Map<TransacoesDTO>(createdEntity);
    }

    public async Task<IEnumerable<TransacoesDTO>> GetAll()
    {
        var transacoesEntity = await _transacoesRepository.GetAll();
        return _mapper.Map<IEnumerable<TransacoesDTO>>(transacoesEntity);
    }

    public async Task<TransacoesDTO> GetTransacoesDTOByIdAsync(string id)
    {
        var transacoesEntity = await _transacoesRepository.GetTransacoesByIdAsync(id);
        return _mapper.Map<TransacoesDTO>(transacoesEntity);
    }

    public async Task<TransacoesDTO> UpdateTransacoesDTOAsync(TransacoesDTO transacoesDTO)
    {
        var transacoesEntity = _mapper.Map<Transacoes>(transacoesDTO);
        var updatedEntity = await _transacoesRepository.UpdateTransacoesAsync(transacoesEntity);
        return _mapper.Map<TransacoesDTO>(updatedEntity);
    }

    public async Task<TransacoesDTO> DeleteTransacoesDTOAsync(string id)
    {
        var transacoesEntity = await _transacoesRepository.DeleteTransacoesAsync(id);
        return _mapper.Map<TransacoesDTO>(transacoesEntity);
    }
}