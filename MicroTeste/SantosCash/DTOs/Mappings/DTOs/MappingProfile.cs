using AutoMapper;
using MicroTeste.Models;

namespace DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamento de criação
        CreateMap<TransacoesCreateRequestDTO, Transacoes>();
        
        // Mapeamento de atualização
        CreateMap<TransacoesUpdateDTO, Transacoes>();

        // Mapeamento para leitura
        CreateMap<Transacoes, TransacoesDTO>();

        // Mapeamento para resposta de criação
        CreateMap<Transacoes, TransacoesCreateResponseDTO>();
    }
}


        
