using AccounteeDomain.Entities;

namespace AccounteeService.Repositories.Interfaces;

public interface IProductRepository
{
    IQueryable<ProductEntity> QueryAll(bool track);
    Task<ProductEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken);
    Task<ProductEntity?> GetByName(string name, bool track, bool allowNull, CancellationToken cancellationToken);
    Task AddProduct(ProductEntity toAdd, bool save, CancellationToken cancellationToken);
    Task DeleteProduct(ProductEntity toDelete, bool save, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}