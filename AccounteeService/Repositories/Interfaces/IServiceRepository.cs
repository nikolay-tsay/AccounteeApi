using AccounteeDomain.Entities;

namespace AccounteeService.Repositories.Interfaces;

public interface IServiceRepository
{
    IQueryable<ServiceEntity> QueryAll(bool track);
    Task<ServiceEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken);
    Task<ServiceEntity?> GetByIdWithProductList(int id, bool track, bool allowNull, CancellationToken cancellationToken);
    Task<ServiceEntity?> GetByName(string name, bool track, bool allowNull, CancellationToken cancellationToken);
    Task AddService(ServiceEntity toAdd, bool save, CancellationToken cancellationToken);
    Task DeleteService(ServiceEntity toDelete, bool save, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}