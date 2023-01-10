using AccounteeCommon.Enums;
using AccounteeDomain.Entities;
using AccounteeService.Contracts.Models;

namespace AccounteeService.PrivateServices.Interfaces;

public interface ICurrentUserPrivateService
{
    Task<CurrentUser> GetCurrentUser(bool tracking, CancellationToken cancellationToken);
    int GetCurrentUserId();
    void CheckUserRights(UserEntity user, UserRights toCheck);
    Task CheckCurrentUserRights(UserRights toCheck, CancellationToken cancellationToken);
}