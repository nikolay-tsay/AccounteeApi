using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AutoMapper;

namespace AccounteeService.Mapping;

public class ModelToEntityProfile : Profile
{
    public ModelToEntityProfile()
    {
        CreateMap<RoleDto, RoleEntity>();
    }
}