using AutoMapper;
using Identity.Domain.ResponseDTOs;
using Identity.Domain.Models;

namespace Identity.API.DependencyInjection;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AuthCustomerDto, Registration>();
    }
}