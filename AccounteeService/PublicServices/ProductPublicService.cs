using AccounteeCommon.Enums;
using AccounteeDomain.Contexts;
using AccounteeDomain.Models;
using AccounteeService.Contracts.Filters;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeCommon.Resources;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class ProductPublicService : IProductPublicService
{
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private AccounteeContext AccounteeContext { get; }
    private IMapper Mapper { get; }
    public ProductPublicService(ICurrentUserPrivateService currentUserPrivateService, AccounteeContext accounteeContext, IMapper mapper)
    {
        CurrentUserPrivateService = currentUserPrivateService;
        AccounteeContext = accounteeContext;
        Mapper = mapper;
    }

    public async Task<PagedList<ProductDto>> GetProducts(string? searchValue, OrderFilter orderFilter, PageFilter pageFilter, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadProducts, cancellationToken);

        var products = await AccounteeContext.Products
            .AsNoTracking()
            .ApplySearch(searchValue)
            .FilterOrder(orderFilter)
            .ToPagedList(pageFilter, cancellationToken);

        var mapped = Mapper.Map<PagedList<ProductEntity>, PagedList<ProductDto>>(products);

        return mapped;
    }

    public async Task<ProductDto> GetProductById(int productId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadProducts, cancellationToken);
        
        var product = await AccounteeContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .Where(x => x.Id == productId)
            .FirstOrNotFound(cancellationToken);

        var mapped = Mapper.Map<ProductDto>(product);

        return mapped;
    }

    public async Task<ProductDto> CreateProduct(ProductDto model, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(false, cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser.User, UserRights.CanCreateProducts);

        var newProduct = Mapper.Map<ProductEntity>(model);
        if (newProduct == null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                nameof(Resources.MappingError), new object[] {nameof(ProductDto), nameof(ProductEntity)}));
        }

        newProduct.IdCompany = GlobalHttpContext.GetCompanyId();

        AccounteeContext.Products.Add(newProduct);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<ProductDto>(newProduct);

        return mapped;
    }

    public async Task<ProductDto> EditProduct(int productId, ProductDto model, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditProducts, cancellationToken);
        
        var product = await AccounteeContext.Products
            .Where(x => x.Id == productId)
            .FirstOrNotFound(cancellationToken);
        
        product.Name = model.Name ?? product.Name;
        product.Description = model.Description ?? product.Description;
        product.AmountUnit = model.AmountUnit ?? product.AmountUnit;
        product.Amount = model.Amount ?? product.Amount;
        product.TotalPrice = model.TotalPrice ?? product.TotalPrice;

        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<ProductDto>(product);

        return mapped;
    }

    public async Task<ProductDto> ChangeProductCategory(int productId, int categoryId,
        CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditProducts, cancellationToken);
        
        var product = await AccounteeContext.Products
            .Include(x => x.ProductCategory)
            .Where(x => x.Id == productId)
            .FirstOrNotFound(cancellationToken);

        var category = await AccounteeContext.Categories
            .Where(x => x.Target == CategoryTargets.Product)
            .Where(x => x.Id == categoryId)
            .FirstOrNotFound(cancellationToken);

        product.IdCategory = category.Id;
        
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<ProductDto>(product);

        return mapped;
    }

    public async Task<bool> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanDeleteProducts, cancellationToken);
        
        var product = await AccounteeContext.Products
            .Where(x => x.Id == productId)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.Products.Remove(product);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}