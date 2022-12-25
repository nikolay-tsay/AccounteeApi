using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AutoMapper;

namespace AccounteeService.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserEntity, UserDto>();
        CreateMap<PagedList<UserEntity>, PagedList<UserDto>>()
            .ForMember(d => d.Items, 
                opt => opt.MapFrom(s => s.Items));
        
        CreateMap<RoleEntity, RoleDto>();
        CreateMap<PagedList<RoleEntity>, PagedList<RoleDto>>()
            .ForMember(d => d.Items, 
                opt => opt.MapFrom(s => s.Items));
        
        CreateMap<CompanyEntity, CompanyDto>();
    }
}