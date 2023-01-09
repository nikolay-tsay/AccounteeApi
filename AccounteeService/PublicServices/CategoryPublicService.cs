using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class CategoryPublicService : ICategoryPublicService
{
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private AccounteeContext AccounteeContext { get; }
    private IMapper Mapper { get; }

    public CategoryPublicService(AccounteeContext accounteeContext, ICurrentUserPrivateService currentUserPrivateService, IMapper mapper)
    {
        AccounteeContext = accounteeContext;
        CurrentUserPrivateService = currentUserPrivateService;
        Mapper = mapper;
    }
    
    public async Task<PagedList<CategoryDto>> GetCategories(OrderFilter orderFilter, PageFilter pageFilter, CategoryTargets target, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadCategories, cancellationToken);

        var categories = await AccounteeContext.Categories
            .AsNoTracking()
            .Where(x => x.Target == target)
            .FilterOrder(orderFilter)
            .ToPagedList(pageFilter, cancellationToken);

        var mapped = Mapper.Map<PagedList<CategoryDto>>(categories);

        return mapped;
    }

    public async Task<CategoryDto> CreateCategory(CategoryDto model, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanCreateCategories, cancellationToken);

        var newCategory = Mapper.Map<CategoryEntity>(model);
        if (newCategory == null)
        {
            throw new AccounteeException();
        }

        newCategory.IdCompany = GlobalHttpContext.GetCompanyId();

        AccounteeContext.Categories.Add(newCategory);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<CategoryDto>(newCategory);
        return mapped;
    }

    public async Task<CategoryDto> EditCategory(int id, CategoryDto model, CategoryTargets target, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditCategories, cancellationToken);

        var category = await AccounteeContext.Categories
            .Where(x => x.Target == target)
            .Where(x => x.Id == id)
            .FirstOrNotFound(cancellationToken);

        category.Name = model.Name ?? category.Name;
        category.Description = model.Description ?? category.Description;

        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<CategoryDto>(category);
        return mapped;
    }

    public async Task<bool> DeleteCategory(CategoryTargets target, int id, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanDeleteCategories, cancellationToken);

        var category = await AccounteeContext.Categories
            .IncludeIf(target == CategoryTargets.Income, x => x.IncomeList)
            .IncludeIf(target == CategoryTargets.Outcome, x => x.OutcomeList)
            .IncludeIf(target == CategoryTargets.Product, x => x.ProductList)
            .IncludeIf(target == CategoryTargets.Service, x => x.ServiceList)
            .Where(x => x.Target == target)
            .Where(x => x.Id == id)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.Categories.Remove(category);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}