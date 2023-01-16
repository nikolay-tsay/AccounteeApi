using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Enums;

namespace AccounteeService.Repositories.Interfaces;

public interface ICategoryRepository
{
    IQueryable<CategoryEntity> GetByTarget(CategoryTargets target);
    Task<CategoryEntity?> GetById(int id, CategoryTargets target, bool track, bool allowNull, CancellationToken cancellationToken);
    Task<CategoryEntity?> GetByName(string name, CategoryTargets target, bool track, bool allowNull, CancellationToken cancellationToken);
    Task AddCategory(CategoryEntity toAdd, bool save, CancellationToken cancellationToken);
    Task DeleteCategory(CategoryEntity toDelete, bool save, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}