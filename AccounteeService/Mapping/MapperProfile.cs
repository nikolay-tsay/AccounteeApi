using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AutoMapper;

namespace AccounteeService.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserEntity, UserDto>();
        CreateMap<RoleEntity, RoleDto>();
        CreateMap<CompanyEntity, CompanyDto>();
    }
}