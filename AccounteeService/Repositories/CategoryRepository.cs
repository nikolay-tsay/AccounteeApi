using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly AccounteeContext _context;

    public CategoryRepository(AccounteeContext context)
    {
        _context = context;
    }
    
    public IQueryable<CategoryEntity> GetByTarget(CategoryTargets target)
    {
        var query = _context.Categories
            .AsNoTracking()
            .Where(x => x.Target == target);

        return query;
    }

    public async Task<CategoryEntity?> GetById(int id, CategoryTargets target, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .TrackIf(track)
            .Where(x => x.Target == target)
            .Where(x => x.Id == id)
            .FirstAllowNull(allowNull, cancellationToken);

        return category;
    }

    public async Task<CategoryEntity?> GetByName(string name, CategoryTargets target, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .TrackIf(track)
            .Where(x => x.Target == target)
            .Where(x => x.Name == name)
            .FirstAllowNull(allowNull, cancellationToken);

        return category;
    }

    public async Task AddCategory(CategoryEntity toAdd, bool save, CancellationToken cancellationToken)
    {
        if (save)
        {
            _context.Categories.Add(toAdd);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        await _context.Categories.AddAsync(toAdd, cancellationToken);
    }

    public async Task DeleteCategory(CategoryEntity toDelete, bool save, CancellationToken cancellationToken)
    {
        _context.Categories.Remove(toDelete);
        
        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}