using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Repositories;

public sealed class ServiceRepository : IServiceRepository
{
    private readonly AccounteeContext _context;

    public ServiceRepository(AccounteeContext context)
    {
        _context = context;
    }
    
    public IQueryable<ServiceEntity> QueryAll(bool track)
    {
        var query = _context.Services
            .TrackIf(track)
            .Include(x => x.ServiceCategory);

        return query;
    }

    public async Task<ServiceEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Services
            .TrackIf(track)
            .Include(x => x.ServiceCategory)
            .Where(x => x.Id == id)
            .FirstAllowNullAsync(allowNull, cancellationToken);

        return result;
    }
    
    public async Task<ServiceEntity?> GetByIdWithProductList(int id, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Services
            .TrackIf(track)
            .Include(x => x.ServiceProductList)!
                .ThenInclude(x => x.Product)
            .Where(x => x.Id == id)
            .FirstAllowNullAsync(allowNull, cancellationToken);

        return result;
    }

    public async Task<ServiceEntity?> GetByName(string name, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Services
            .TrackIf(track)
            .Where(x => x.Name == name)
            .FirstAllowNullAsync(allowNull, cancellationToken);

        return result;
    }

    public async Task AddService(ServiceEntity toAdd, bool save, CancellationToken cancellationToken)
    {
        if (save)
        {
            _context.Services.Add(toAdd);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        await _context.Services.AddAsync(toAdd, cancellationToken);
    }

    public async Task DeleteService(ServiceEntity toDelete, bool save, CancellationToken cancellationToken)
    {
        _context.Services.Remove(toDelete);

        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}