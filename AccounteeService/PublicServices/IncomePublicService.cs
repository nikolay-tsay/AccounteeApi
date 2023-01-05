using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Relational;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Models;
using AccounteeService.Contracts.Requests;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class IncomePublicService : IIncomePublicService
{
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private AccounteeContext AccounteeContext { get; }
    private IMapper Mapper { get; }
    public IncomePublicService(ICurrentUserPrivateService currentUserPrivateService, AccounteeContext accounteeContext, IMapper mapper)
    {
        CurrentUserPrivateService = currentUserPrivateService;
        AccounteeContext = accounteeContext;
        Mapper = mapper;
    }
    
    public async Task<PagedList<IncomeDto>> GetIncomes(PageFilter filter, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadOutlay, cancellationToken);

        var incomes = await AccounteeContext.Incomes
            .AsNoTracking()
            .ToPagedList(filter, cancellationToken);

        var mapped = Mapper.Map<PagedList<IncomeEntity>, PagedList<IncomeDto>>(incomes);

        return mapped;
    }

    public async Task<PagedList<IncomeDto>> GetUserIncomes(int? userId, PageFilter filter, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadOutlay, cancellationToken);
        
        var userIncomes = await AccounteeContext.UserIncomes
            .AsNoTracking()
            .Include(x => x.Income)
            .Where(x => x.IdUser == userId)
            .Select(x => x.Income)
            .ToPagedList(filter, cancellationToken);

        var mapped = Mapper.Map<PagedList<IncomeEntity>, PagedList<IncomeDto>>(userIncomes);

        return mapped;
    }

    public async Task<IncomeDetailModel> GetIncomeDetails(int incomeId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadOutlay, cancellationToken);
        
        var income = await AccounteeContext.Incomes
            .AsNoTracking()
            .Include(x => x.IncomeCategory)
            .Include(x => x.Service)
            .Include(x => x.UserIncomeList)!
                .ThenInclude(x => x.User)
            .Include(x => x.IncomeProductList)!
                .ThenInclude(x => x.Product)
            .Where(x => x.Id == incomeId)
            .AsSplitQuery()
            .FirstOrNotFound(cancellationToken);

        var mapped = Mapper.Map<IncomeDetailModel>(income);

        return mapped;
    }

    public async Task<bool> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanCreateOutlay, cancellationToken);
        
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new AccounteeException();
        }
        
        var newIncome = new IncomeEntity
        {
            IdCompany = GlobalHttpContext.GetCompanyId(),
            IdCategory = request.IdCategory,
            IdService = request.IdService,
            Name = request.Name,
            Description = request.Description,
            DateTime = request.DateTime,
            LastEdited = DateTime.UtcNow
        };

        if (newIncome.IdService == null)
        {
            await FillProductList(request.ProductToIncomeRequests!, newIncome, cancellationToken);
        }
        else
        {
            await CalculateServiceIncome(newIncome, cancellationToken);
        }
        
        await FillUserList(request.UserToIncomeRequests!, newIncome, cancellationToken);

        if (newIncome == null)
        {
            throw new AccounteeException();
        }

        AccounteeContext.Add(newIncome);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IncomeDetailModel> EditIncome(IncomeDto model, int incomeId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditOutlay, cancellationToken);
        
        var income = await AccounteeContext.Incomes
            .Where(x => x.Id == incomeId)
            .FirstOrNotFound(cancellationToken);

        income.Name = model.Name ?? income.Name;
        income.Description = model.Description ?? income.Description;
        income.LastEdited = DateTime.UtcNow;
        income.TotalAmount = model.TotalAmount ?? income.TotalAmount;

        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<IncomeDetailModel>(income);

        return mapped;
    }

    public async Task<IncomeDetailModel> AddProductToIncome(int incomeId, int productId,
        CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditOutlay, cancellationToken);
        
        var income = await AccounteeContext.Incomes
            .Include(x => x.IncomeCategory)
            .Include(x => x.IncomeProductList)
            .Where(x => x.Id == incomeId)
            .FirstOrNotFound(cancellationToken);

        var product = await AccounteeContext.Products
            .Where(x => x.Id == productId)
            .FirstOrNotFound(cancellationToken);

        var newIncomeProductEntity = new IncomeProductEntity
        {
            IdIncome = income.Id,
            IdProduct = product.Id,
            IdCompany = GlobalHttpContext.GetCompanyId(),
            Amount = product.Amount
        };

        AccounteeContext.IncomeProducts.Add(newIncomeProductEntity);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<IncomeDetailModel>(income);

        return mapped;
    }

    public async Task<IncomeDetailModel> DeleteProductFromIncome(int incomeId, int productId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditOutlay, cancellationToken);
        
        var incomeProduct = await AccounteeContext.IncomeProducts
            .Where(x => x.IdIncome == incomeId)
            .Where(x => x.IdProduct == productId)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.IncomeProducts.Remove(incomeProduct);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var income = await AccounteeContext.Incomes
            .AsNoTracking()
            .Include(x => x.IncomeCategory)
            .Include(x => x.IncomeProductList)
            .Where(x => x.Id == incomeId)
            .FirstOrNotFound(cancellationToken);

        var mapped = Mapper.Map<IncomeDetailModel>(income);

        return mapped;
    }
    
     private async Task FillProductList(IEnumerable<ProductToIncomeRequest> requests,
        IncomeEntity income, CancellationToken cancellationToken)
    {
        foreach (var request in requests)
        {
            var product = await AccounteeContext.Products
                .Where(x => x.Id == request.Id)
                .FirstOrNotFound(cancellationToken);

            var newIncomeProductEntity = new IncomeProductEntity
            {
                IdCompany = income.IdCompany,
                IdProduct = product.Id,
                Income = income,
                Amount = request.Amount
            };

            await AccounteeContext.IncomeProducts.AddAsync(newIncomeProductEntity, cancellationToken);

            product.Amount -= newIncomeProductEntity.Amount;
            income.TotalAmount += product.TotalPrice; 
        }
    }

    private async Task CalculateServiceIncome(IncomeEntity income, CancellationToken cancellationToken)
    {
        var service = await AccounteeContext.Services
            .AsNoTracking()
            .Where(x => x.Id == income.IdService)
            .FirstOrNotFound(cancellationToken);

        income.TotalAmount = service.TotalPrice;

        if (service.ServiceProductList == null)
        {
            return;
        }
        
        foreach (var serviceProduct in service.ServiceProductList)
        {
            var product = await AccounteeContext.Products
                .Where(x => x.Id == serviceProduct.IdProduct)
                .FirstOrNotFound(cancellationToken);

            product.Amount -= serviceProduct.ProductUsedAmount ?? 0;
        }
    }

    private async Task FillUserList(IEnumerable<UserToIncomeRequest> requests,
        IncomeEntity income, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);

        decimal globalIncomePercent = 0;
        
        if (currentUser.IncomePercent != null)
        {
            var newCurrentUserIncomeEntity = new UserIncomeEntity
            {
                IdCompany = income.IdCompany,
                IdUser = currentUser.Id,
                Income = income,
                Amount = income.TotalAmount * currentUser.IncomePercent.Value / 100
            };

            globalIncomePercent += (decimal)currentUser.IncomePercent;
            
            await AccounteeContext.UserIncomes.AddAsync(newCurrentUserIncomeEntity, cancellationToken);
        }

        foreach (var request in requests)
        {
            var user = await AccounteeContext.Users
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .FirstOrNotFound(cancellationToken);

            var percent = request.IncomePercent ?? user.IncomePercent;
            if (percent == null)
            {
                continue;
            }

            globalIncomePercent += (decimal)percent;
            if (globalIncomePercent > 100)
            {
                throw new AccounteeException();
            }
            
            var newUserIncomeEntity = new UserIncomeEntity
            {
                IdCompany = income.IdCompany,
                IdUser = user.Id,
                Income = income,
                Amount = income.TotalAmount * percent.Value / 100
            };
            
            await AccounteeContext.UserIncomes.AddAsync(newUserIncomeEntity, cancellationToken);
        }
    }
}