using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Relational;

namespace AccounteeService.Repositories.Interfaces;

public interface IIncomeRepository
{
    IQueryable<IncomeEntity> QueryAll(bool track);
    IQueryable<IncomeEntity> QueryByUser(int userId, bool track);
    Task<IncomeEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken);
    Task<IncomeEntity?> GetByIdDetail(int id, bool track, bool allowNull, CancellationToken cancellationToken);
    Task AddProductToIncome(IncomeEntity entity, ProductEntity product, decimal amount, bool save, CancellationToken cancellationToken);
    Task DeleteProductFromIncome(IncomeEntity entity, IncomeProductEntity toDelete, decimal amount, bool save, CancellationToken cancellationToken);
    Task CalcServiceIncome(IncomeEntity entity, ServiceEntity service, bool save, CancellationToken cancellationToken);
    Task AddUserIncome(IncomeEntity entity, UserEntity user, decimal? percent, bool save, CancellationToken cancellationToken);
    Task AddIncome(IncomeEntity toAdd, bool save, CancellationToken cancellationToken);
    Task DeleteIncome(IncomeEntity toDelete, bool save, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}