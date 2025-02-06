using AutoMapper;
using DTOs;
using Models;

namespace Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamento de criação
        CreateMap<TransacoesCreateRequestDTO, Transacoes>().ReverseMap();
        
        // Mapeamento de atualização
        CreateMap<TransacoesUpdateDTO, Transacoes>().ReverseMap();

        // Mapeamento para leitura
        CreateMap<Transacoes, TransacoesDTO>().ReverseMap();

        // Mapeamento para resposta de criação
        CreateMap<Transacoes, TransacoesCreateResponseDTO>().ReverseMap();
    }
}


        
