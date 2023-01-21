using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;

namespace AccounteeService.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly AccounteeContext _context;

    public RoleRepository(AccounteeContext context)
    {
        _context = context;
    }
    
    public IQueryable<RoleEntity> QueryAll(bool track)
    {
        var query = _context.Roles
            .TrackIf(track);

        return query;
    }

    public async Task<RoleEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Roles
            .TrackIf(track)
            .Where(x => x.Id == id)
            .FirstAllowNullAsync(allowNull, cancellationToken);

        return result;
    }

    public async Task<RoleEntity?> GetByName(string name, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await _context.Roles
            .TrackIf(track)
            .Where(x => x.Name == name)
            .FirstAllowNullAsync(allowNull, cancellationToken);

        return result;
    }

    public async Task AddRole(RoleEntity toAdd, bool save, CancellationToken cancellationToken)
    {
        _context.Roles.Add(toAdd);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRole(RoleEntity toDelete, bool save, CancellationToken cancellationToken)
    {
        _context.Roles.Remove(toDelete);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}