using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Relational;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Models;
using AutoMapper;

namespace AccounteeService.Mapping;

public class EntityToModelProfile : Profile
{
    public EntityToModelProfile()
    {
        CreateMap<CompanyEntity, CompanyDto>();
        
        CreateMap<UserEntity, UserDto>();
        CreateMap<PagedList<UserEntity>, PagedList<UserDto>>()
            .ForMember(d => d.Items, 
                opt 
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<RoleEntity, RoleDto>();
        CreateMap<PagedList<RoleEntity>, PagedList<RoleDto>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));

        CreateMap<IncomeEntity, IncomeDto>()
            .ForMember(x => x.CategoryName, opt 
                => opt.MapFrom(x => x.IncomeCategory.Name))
            .ForMember(x => x.ServiceName, opt
                => opt.MapFrom(x => x.Service != null ? x.Service.Name : string.Empty));
        
        CreateMap<PagedList<IncomeEntity>, PagedList<IncomeDto>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<ProductEntity, ProductDto>()
            .ForMember(x => x.CategoryName, opt 
                => opt.MapFrom(x => x.ProductCategory.Name));
        
        CreateMap<PagedList<ProductEntity>, PagedList<ProductDto>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<OutcomeEntity, OutcomeDto>()
            .ForMember(x => x.CategoryName, opt 
                => opt.MapFrom(x => x.OutcomeCategory.Name));
        
        CreateMap<PagedList<OutcomeEntity>, PagedList<OutcomeDto>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
        
        CreateMap<ServiceEntity, ServiceDto>()
            .ForMember(x => x.CategoryName, opt 
                => opt.MapFrom(x => x.ServiceCategory.Name));

        CreateMap<PagedList<ServiceEntity>, PagedList<ServiceDto>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));

        CreateMap<IncomeProductEntity, IncomeProductDto>()
            .ForMember(x => x.ProductName, opt
                => opt.MapFrom(x => x.Product.Name));
        
        CreateMap<UserIncomeEntity, UserIncomeDto>()
            .ForMember(x => x.FirstName, opt
                => opt.MapFrom(x => x.User.FirstName))
            .ForMember(x => x.LastName, opt
                => opt.MapFrom(x => x.User.LastName));

        CreateMap<IncomeEntity, IncomeDetailModel>()
            .ForMember(x => x.CategoryName, opt
                => opt.MapFrom(x => x.IncomeCategory.Name))
            .ForMember(x => x.ServiceName, opt
                => opt.MapFrom(x => x.Service != null ? x.Service.Name : string.Empty))
            .ForMember(x => x.UserList, opt 
                => opt.MapFrom(x => x.UserIncomeList))
            .ForMember(x => x.ProductList, opt
                => opt.MapFrom(x => x.IncomeProductList));
        
        CreateMap<CategoryEntity, CategoryDto>();
        
        CreateMap<PagedList<CategoryEntity>, PagedList<CategoryDto>>()
            .ForMember(d => d.Items, 
                opt
                    => opt.MapFrom(s => s.Items));
    }
}