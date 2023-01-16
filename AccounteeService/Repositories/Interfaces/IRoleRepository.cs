using AccounteeDomain.Entities;

namespace AccounteeService.Repositories.Interfaces;

public interface IRoleRepository
{
    IQueryable<RoleEntity> QueryAll(bool track);
    Task<RoleEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken);
    Task<RoleEntity?> GetByName(string name, bool track, bool allowNull, CancellationToken cancellationToken);
    Task AddRole(RoleEntity toAdd, bool save, CancellationToken cancellationToken);
    Task DeleteRole(RoleEntity toDelete, bool save, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}