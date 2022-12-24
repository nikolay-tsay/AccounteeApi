using AccounteeDomain.Entities;
using AccounteeService.Contracts.Enums;

namespace AccounteeService.PrivateServices.Interfaces;

public interface ICurrentUserPrivateService
{
    Task<UserEntity> GetCurrentUser(CancellationToken cancellationToken);
    int GetCurrentUserId();
    void CheckUserRights(UserEntity user, UserRights toCheck);
    Task CheckCurrentUserRights(UserRights toCheck, CancellationToken cancellationToken);
}