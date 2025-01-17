using AutoMapper;
using MicroTeste.Models;

namespace DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transacoes, TransacoesDTO>().ReverseMap();
        CreateMap<Keys, KeysDTO>().ReverseMap();
    }
}
