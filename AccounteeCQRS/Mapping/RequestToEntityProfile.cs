using AccounteeCQRS.Requests.Category;
using AccounteeCQRS.Requests.Product;
using AccounteeCQRS.Requests.Role;
using AccounteeCQRS.Requests.Service;
using AccounteeDomain.Entities;
using AutoMapper;

namespace AccounteeCQRS.Mapping;

public sealed class RequestToEntityProfile : Profile
{
    public RequestToEntityProfile()
    {
        CreateMap<CreateCategoryCommand, CategoryEntity>();
        CreateMap<CreateRoleCommand, RoleEntity>();
        CreateMap<CreateProductCommand, ProductEntity>();
        CreateMap<CreateServiceCommand, ServiceEntity>();
    }
}