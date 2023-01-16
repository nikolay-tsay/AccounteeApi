using AccounteeDomain.Entities;

namespace AccounteeService.Repositories.Interfaces;

public interface IUserRepository
{
    IQueryable<UserEntity> QueryAll(bool track);
    Task<UserEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken);
    Task<UserEntity?> GetByLogin(string login, bool track, bool allowNull, CancellationToken cancellationToken);
    Task AddUser(UserEntity user, bool save, CancellationToken cancellationToken);
    Task DeleteUser(UserEntity user, bool save, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}