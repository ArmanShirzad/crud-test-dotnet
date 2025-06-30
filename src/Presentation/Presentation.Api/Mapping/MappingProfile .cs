using AutoMapper;
using Presentation.Shared.Models;
using Core.Domain.Entities;

namespace Presentation.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // DTO to Entity
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.PhoneNumber) ? 0UL : ulong.Parse(src.PhoneNumber)));

            // Entity to DTO
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.ToString()));
        }
    }
}
