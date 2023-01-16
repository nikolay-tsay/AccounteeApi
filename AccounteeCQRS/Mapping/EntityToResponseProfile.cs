using AccounteeCQRS.Responses;
using AccounteeCQRS.Responses.Income;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Relational;
using AccounteeService.Contracts;
using AutoMapper;

namespace AccounteeCQRS.Mapping;

public sealed class EntityToResponseProfile : Profile
{
    public EntityToResponseProfile()
    {
        CreateMap<UserEntity, RegisterResponse>();
        CreateMap<CategoryEntity, CategoryResponse>();
        CreateMap<PagedList<CategoryEntity>, PagedList<CategoryResponse>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));

        CreateMap<RoleEntity, RoleResponse>();
        CreateMap<PagedList<RoleEntity>, PagedList<RoleResponse>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<ProductEntity, ProductResponse>()
            .ForMember(x => x.CategoryName, opt 
                => opt.MapFrom(x => x.ProductCategory.Name));
        
        CreateMap<PagedList<ProductEntity>, PagedList<ProductResponse>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<ServiceEntity, ServiceResponse>()
            .ForMember(x => x.CategoryName, opt 
                => opt.MapFrom(x => x.ServiceCategory.Name));
        
        CreateMap<PagedList<ServiceEntity>, PagedList<ServiceResponse>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<UserEntity, UserResponse>()
            .ForMember(x => x.RoleName, opt 
                => opt.MapFrom(x => x.Role.Name));
        
        CreateMap<PagedList<UserEntity>, PagedList<UserResponse>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));

        CreateMap<CompanyEntity, CompanyResponse>();
        
        CreateMap<UserIncomeEntity, IncomeUserResponse>();
        CreateMap<IncomeProductEntity, IncomeProductResponse>();
        
        CreateMap<IncomeEntity, IncomeResponse>()
            .ForMember(x => x.CategoryName, opt
                => opt.MapFrom(x => x.IncomeCategory.Name))
            .ForMember(x => x.ServiceName, opt 
                => opt.MapFrom(x => x.Service == null 
                    ? string.Empty 
                    : x.Service.Name));
        
        CreateMap<PagedList<IncomeEntity>, PagedList<IncomeResponse>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<IncomeEntity, IncomeDetailResponse>()
            .ForMember(x => x.CategoryName, opt
                => opt.MapFrom(x => x.IncomeCategory.Name))
            .ForMember(x => x.ServiceName, opt 
                => opt.MapFrom(x => x.Service == null 
                    ? string.Empty 
                    : x.Service.Name))
            .ForMember(x => x.ProductList, opt 
                => opt.MapFrom(x => x.IncomeProductList))
            .ForMember(x => x.UserList, opt 
                => opt.MapFrom(x => x.UserIncomeList));
    }
}