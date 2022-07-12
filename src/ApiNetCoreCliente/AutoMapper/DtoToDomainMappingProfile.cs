using AutoMapper;
using ApiNetCoreCliente.Domain.DTOs;
using ApiNetCoreCliente.Domain.Entities;

namespace ApiNetCoreCliente.AutoMapper;

public class DtoToDomainMappingProfile : Profile
{
    public DtoToDomainMappingProfile()
    {
        CreateMap<ClienteDto, Cliente>();
    }
}
