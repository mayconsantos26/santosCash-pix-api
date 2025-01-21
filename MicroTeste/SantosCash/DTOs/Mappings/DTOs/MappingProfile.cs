using AutoMapper;
using MicroTeste.Models;

namespace DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeia TransacoesDTO para Transacoes
        CreateMap<TransacoesDTO, Transacoes>().ReverseMap();

        // Mapeia Transacoes para TransacoesDTO
        CreateMap<Transacoes, TransacoesDTO>().ReverseMap();

        // Mapeamento da entidade Transacoes para TransacoesDTO
        CreateMap<Transacoes, TransacoesDTO>().ReverseMap();

        // Mapeamento para criação de transação (Request para Entidade)
        CreateMap<TransacoesCreateRequestDTO, Transacoes>().ReverseMap();

        // Mapeamento para resposta de transação (Entidade para Response)
        CreateMap<Transacoes, TransacoesCreateResponseDTO>().ReverseMap();
    }
}

        
