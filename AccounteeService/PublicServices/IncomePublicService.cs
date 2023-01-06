using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
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
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanEditOutlay);
        
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new AccounteeException();
        }
        
        var newIncome = new IncomeEntity
        {
            IdCompany = currentUser.IdCompany,
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

    public async Task<bool> DeleteIncome(int incomeId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanDeleteOutlay, cancellationToken);
        
        var income = await AccounteeContext.Incomes
            .Where(x => x.Id == incomeId)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.Incomes.Remove(income);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IncomeDetailModel> AddProductToIncome(int incomeId, IEnumerable<ProductToIncomeRequest> requests,
        CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanEditOutlay);
        
        var income = await AccounteeContext.Incomes
            .Include(x => x.IncomeCategory)
            .Include(x => x.IncomeProductList)
            .Where(x => x.Id == incomeId)
            .FirstOrNotFound(cancellationToken);

        foreach (var request in requests)
        {
            var currentIncomeProduct = income.IncomeProductList!
                .FirstOrDefault(x => x.IdProduct == request.Id);
            
            if (currentIncomeProduct == null)
            {
                var newProduct = await AccounteeContext.Products
                    .Where(x => x.Id == request.Id)
                    .FirstOrNotFound(cancellationToken);

                var newIncomeProductEntity = new IncomeProductEntity
                {
                    IdCompany = income.IdCompany,
                    IdProduct = newProduct.Id,
                    Income = income,
                    Amount = request.Amount
                };

                await AccounteeContext.IncomeProducts.AddAsync(newIncomeProductEntity, cancellationToken);

                newProduct.Amount -= newIncomeProductEntity.Amount;
                income.TotalAmount += newProduct.TotalPrice * newIncomeProductEntity.Amount; 
                continue;
            }

            var existingProduct = await AccounteeContext.Products
                .Where(x => x.Id == currentIncomeProduct.IdProduct)
                .FirstOrNotFound(cancellationToken);

            existingProduct.Amount -= request.Amount;
            currentIncomeProduct.Amount += request.Amount;
            income.TotalAmount += existingProduct.TotalPrice * request.Amount;
        }
       
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<IncomeDetailModel>(income);

        return mapped;
    }

    public async Task<IncomeDetailModel> DeleteProductFromIncome(int incomeId, 
        IEnumerable<ProductToIncomeRequest> requests, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditOutlay, cancellationToken);
        
        var income = await AccounteeContext.Incomes
            .Include(x => x.IncomeCategory)
            .Include(x => x.IncomeProductList)
            .Where(x => x.Id == incomeId)
            .FirstOrNotFound(cancellationToken);

        if (income.IncomeProductList == null)
        {
            throw new AccounteeException();
        }

        foreach (var request in requests)
        {
            var toDelete = income.IncomeProductList
                .FirstOrDefault(x => x.IdProduct == request.Id);

            if (toDelete == null || toDelete.Amount < request.Amount)
            {
                throw new AccounteeException();
            }

            toDelete.Amount -= request.Amount;
            
            var product = await AccounteeContext.Products
                .Where(x => x.Id == toDelete.IdProduct)
                .FirstOrNotFound(cancellationToken);

            if (toDelete.Amount == 0)
            {
                product.Amount += request.Amount;
                income.TotalAmount -= toDelete.Product.TotalPrice * request.Amount;
                AccounteeContext.IncomeProducts.Remove(toDelete);
                continue;
            }
            
            product.Amount += request.Amount;
            income.TotalAmount -= toDelete.Product.TotalPrice * request.Amount;
        }

        await AccounteeContext.SaveChangesAsync(cancellationToken);

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
            income.TotalAmount += product.TotalPrice * newIncomeProductEntity.Amount; 
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

            globalIncomePercent += currentUser.IncomePercent.Value;
            
            await AccounteeContext.UserIncomes.AddAsync(newCurrentUserIncomeEntity, cancellationToken);
        }

        foreach (var request in requests)
        {
            if (globalIncomePercent > 100)
            {
                throw new AccounteeException();
            }
            
            if (request.Id == currentUser.Id)
            {
                continue;
            }
            
            var user = await AccounteeContext.Users
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .FirstOrNotFound(cancellationToken);

            var percent = request.IncomePercent ?? user.IncomePercent;
            if (percent == null)
            {
                continue;
            }
            
            var newUserIncomeEntity = new UserIncomeEntity
            {
                IdCompany = income.IdCompany,
                IdUser = user.Id,
                Income = income,
                Amount = income.TotalAmount * percent.Value / 100
            };
            
            await AccounteeContext.UserIncomes.AddAsync(newUserIncomeEntity, cancellationToken);
            
            globalIncomePercent += percent.Value;
        }
    }
}