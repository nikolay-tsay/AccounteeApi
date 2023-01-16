using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Relational;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Repositories;

public sealed class IncomeRepository : IIncomeRepository
{
    private readonly AccounteeContext _context;

    public IncomeRepository(AccounteeContext context)
    {
        _context = context;
    }
    
    public IQueryable<IncomeEntity> QueryAll(bool track)
    {
        var query = _context.Incomes
            .TrackIf(track)
            .Include(x => x.IncomeCategory)
            .Include(x => x.Service);

        return query;
    }

    public IQueryable<IncomeEntity> QueryByUser(int userId, bool track)
    {
        var query = _context.UserIncomes
            .AsNoTracking()
            .Include(x => x.Income.IncomeCategory)
            .Include(x => x.Income.Service)
            .Where(x => x.IdUser == userId)
            .Select(x => x.Income);

        return query;
    }

    public async Task<IncomeEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Incomes
            .TrackIf(track)
            .Include(x => x.IncomeCategory)
            .Include(x => x.Service)
            .Where(x => x.Id == id)
            .FirstAllowNull(allowNull, cancellationToken);

        return result;
    }

    public async Task<IncomeEntity?> GetByIdDetail(int id, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Incomes
            .TrackIf(track)
            .Include(x => x.IncomeCategory)
            .Include(x => x.Service)
            .Include(x => x.UserIncomeList)!
                .ThenInclude(x => x.User)
            .Include(x => x.IncomeProductList)!
                .ThenInclude(x => x.Product)
            .Where(x => x.Id == id)
            .AsSplitQuery()
            .FirstAllowNull(allowNull, cancellationToken);

        return result;
    }

    public async Task AddProductToIncome(IncomeEntity entity, ProductEntity product, decimal amount, bool save, CancellationToken cancellationToken)
    {
        var currentIncomeProduct = entity.IncomeProductList!
            .FirstOrDefault(x => x.IdProduct == product.Id);
        
        if (currentIncomeProduct is null)
        {
            await AddNewProduct(entity, product, amount, save, cancellationToken);
            return;
        }

        await AddExistingProduct(entity, currentIncomeProduct, amount, save, cancellationToken);
    }

    public async Task DeleteProductFromIncome(IncomeEntity entity, IncomeProductEntity toDelete, decimal amount, bool save, CancellationToken cancellationToken)
    {
        toDelete.Amount -= amount;
        var product = toDelete.Product;

        if (toDelete.Amount == 0)
        {
            product.Amount += amount;
            entity.TotalAmount -= toDelete.Product.TotalPrice * amount;
            _context.IncomeProducts.Remove(toDelete);
            return;
        }
        
        product.Amount += amount;
        entity.TotalAmount -= toDelete.Product.TotalPrice * amount;

        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task CalcServiceIncome(IncomeEntity entity, ServiceEntity service, bool save, CancellationToken cancellationToken)
    {
        entity.TotalAmount = service.TotalPrice;
        if (service.ServiceProductList is null)
        {
            return;
        }

        foreach (var serviceProduct in service.ServiceProductList)
        {
            serviceProduct.Product.Amount -= serviceProduct.ProductUsedAmount ?? 0;
        }

        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task AddUserIncome(IncomeEntity entity, UserEntity user, decimal? percent,  bool save, CancellationToken cancellationToken)
    {
        var finalPercent = percent ?? user.IncomePercent;
        if (finalPercent is null)
        {
            return;
        }
            
        var newUserIncomeEntity = new UserIncomeEntity
        {
            IdCompany = entity.IdCompany,
            IdUser = user.Id,
            Income = entity,
            Amount = entity.TotalAmount * finalPercent.Value / 100
        };
        
        if (save)
        {
            _context.UserIncomes.Add(newUserIncomeEntity);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        await _context.UserIncomes.AddAsync(newUserIncomeEntity, cancellationToken);
    }

    public async Task AddIncome(IncomeEntity toAdd, bool save, CancellationToken cancellationToken)
    {
        if (save)
        {
            _context.Incomes.Add(toAdd);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        await _context.Incomes.AddAsync(toAdd, cancellationToken);
    }

    public async Task DeleteIncome(IncomeEntity toDelete, bool save, CancellationToken cancellationToken)
    {
        _context.Incomes.Remove(toDelete);
        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
    
    
    private async Task AddExistingProduct(IncomeEntity entity, IncomeProductEntity currentIncomeProduct, decimal amount, bool save,
        CancellationToken cancellationToken)
    {
        currentIncomeProduct.Product.Amount -= amount;
        currentIncomeProduct.Amount += amount;
        entity.TotalAmount += currentIncomeProduct.Product.TotalPrice * amount;

        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task AddNewProduct(IncomeEntity entity, ProductEntity product, decimal amount, bool save, CancellationToken cancellationToken)
    {
        var newIncomeProductEntity = new IncomeProductEntity
        {
            IdCompany = entity.IdCompany,
            IdProduct = product.Id,
            Income = entity,
            Amount = amount
        };
        
        product.Amount -= newIncomeProductEntity.Amount;
        entity.TotalAmount += product.TotalPrice * newIncomeProductEntity.Amount;
        
        if (save)
        {
            _context.IncomeProducts.Add(newIncomeProductEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        await _context.IncomeProducts.AddAsync(newIncomeProductEntity, cancellationToken);
    }
}