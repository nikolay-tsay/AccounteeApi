using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AccounteeContext _context;

    public ProductRepository(AccounteeContext context)
    {
        _context = context;
    }
    
    public IQueryable<ProductEntity> QueryAll(bool track)
    {
        var query = _context.Products
            .TrackIf(track);

        return query;
    }

    public async Task<ProductEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Products
            .TrackIf(track)
            .Include(x => x.ProductCategory)
            .Where(x => x.Id == id)
            .FirstAllowNull(allowNull, cancellationToken);

        return result;
    }

    public async Task<ProductEntity?> GetByName(string name, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Products
            .TrackIf(track)
            .Include(x => x.ProductCategory)
            .Where(x => x.Name == name)
            .FirstAllowNull(allowNull, cancellationToken);

        return result;
    }

    public async Task AddProduct(ProductEntity toAdd, bool save, CancellationToken cancellationToken)
    {
        if (save)
        {
            _context.Products.Add(toAdd);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        await _context.Products.AddAsync(toAdd, cancellationToken);
    }

    public async Task DeleteProduct(ProductEntity toDelete, bool save, CancellationToken cancellationToken)
    {
        _context.Products.Remove(toDelete);

        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}