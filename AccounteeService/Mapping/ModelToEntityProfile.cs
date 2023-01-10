using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.Contracts.Requests;
using AutoMapper;

namespace AccounteeService.Mapping;

public class ModelToEntityProfile : Profile
{
    public ModelToEntityProfile()
    {
        CreateMap<RoleDto, RoleEntity>();
        CreateMap<CategoryDto, CategoryEntity>();
        CreateMap<ProductDto, ProductEntity>();
        CreateMap<CreateIncomeRequest, IncomeEntity>();
        CreateMap<ServiceDto, ServiceEntity>();
    }
}