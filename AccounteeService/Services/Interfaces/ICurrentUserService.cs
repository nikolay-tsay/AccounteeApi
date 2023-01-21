using AccounteeCommon.Enums;
using AccounteeDomain.Entities;
using AccounteeService.Contracts.Models;

namespace AccounteeService.Services.Interfaces;

public interface ICurrentUserService
{
    Task<CurrentUser> GetCurrentUser(bool tracking, CancellationToken cancellationToken);
    void CheckUserRights(UserEntity user, UserRights toCheck);
    Task CheckCurrentUserRights(UserRights toCheck, CancellationToken cancellationToken);
}