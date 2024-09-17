using AutoMapper;
using Identity.Domain.Models;
using Identity.Domain.ResponseDTOs;

namespace Identity.Repositories;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Registration, AuthCustomerDto>()
            .ForMember(dest => dest.CustomerKey, opt => opt.MapFrom(src => src.CustomerKey))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

    }
}