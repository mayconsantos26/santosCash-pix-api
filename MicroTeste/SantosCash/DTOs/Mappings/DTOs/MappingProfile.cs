using AutoMapper;
using DTOs;
using MicroTeste.Models;

namespace SantosCash;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transacoes, TransacoesDTO>().ReverseMap();
        CreateMap<Keys, KeysDTO>().ReverseMap();
    }
}
