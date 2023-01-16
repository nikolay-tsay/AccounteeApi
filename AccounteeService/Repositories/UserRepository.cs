using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AccounteeContext _context;

    public UserRepository(AccounteeContext context)
    {
        _context = context;
    }

    public IQueryable<UserEntity> QueryAll(bool track)
    {
        var query = _context.Users
            .TrackIf(track);

        return query;
    }

    public async Task<UserEntity?> GetById(int id, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .TrackIf(track)
            .Include(x => x.Role)
            .Include(x => x.Company)
            .Where(x => x.Id == id)
            .FirstAllowNull(allowNull, cancellationToken);

        return user;
    }

    public async Task<UserEntity?> GetByLogin(string login, bool track, bool allowNull, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .TrackIf(track)
            .Include(x => x.Role)
            .Include(x => x.Company)
            .Where(x => x.Login == login)
            .FirstAllowNull(allowNull, cancellationToken);

        return user;
    }

    public async Task AddUser(UserEntity user, bool save, CancellationToken cancellationToken)
    {
        if (save)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task DeleteUser(UserEntity user, bool save, CancellationToken cancellationToken)
    {
        _context.Users.Remove(user);

        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}