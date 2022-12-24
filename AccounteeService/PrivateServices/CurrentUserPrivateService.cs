using AccounteeCommon.Exceptions;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeService.Contracts.Enums;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PrivateServices;

public class CurrentUserPrivateService : ICurrentUserPrivateService
{
    private IHttpContextAccessor HttpContextAccessor { get; }
    private AccounteeContext AccounteeContext { get; }

    public CurrentUserPrivateService(IHttpContextAccessor httpContextAccessor, AccounteeContext accounteeContext)
    {
        HttpContextAccessor = httpContextAccessor;
        AccounteeContext = accounteeContext;
    }


    public async Task<UserEntity> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var user = await AccounteeContext.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .Include(x => x.Company)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        return user;
    }

    public void CheckUserRights(UserEntity user,UserRights toCheck)
    {
        bool hasRight = toCheck switch
        {
            UserRights.IsAdmin => user.Role.IsAdmin,
            UserRights.CanCreateCompany => user.Role.CanCreateCompany,
            UserRights.CanEditCompany => user.Role.CanEditCompany,
            UserRights.CanDeleteCompany => user.Role.CanDeleteCompany,
            UserRights.CanRead => user.Role.CanRead,
            UserRights.CanCreate => user.Role.CanCreate,
            UserRights.CanEdit => user.Role.CanEdit,
            UserRights.CanDelete => user.Role.CanDelete,
            UserRights.CanUploadFiles => user.Role.CanUploadFiles,
            _ => false
        };

        if (!hasRight)
        {
            throw new AccounteeUnauthorizedException();
        }
    }

    public async Task CheckCurrentUserRights(UserRights toCheck, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUser(cancellationToken);
        bool hasRight = toCheck switch
        {
            UserRights.IsAdmin => user.Role.IsAdmin,
            UserRights.CanCreateCompany => user.Role.CanCreateCompany,
            UserRights.CanEditCompany => user.Role.CanEditCompany,
            UserRights.CanDeleteCompany => user.Role.CanDeleteCompany,
            UserRights.CanRead => user.Role.CanRead,
            UserRights.CanCreate => user.Role.CanCreate,
            UserRights.CanEdit => user.Role.CanEdit,
            UserRights.CanDelete => user.Role.CanDelete,
            UserRights.CanUploadFiles => user.Role.CanUploadFiles,
            _ => false
        };

        if (!hasRight)
        {
            throw new AccounteeUnauthorizedException();
        }
    }

    public int GetCurrentUserId()
    {
        var claimStr = HttpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimNames.UserId)?.Value;
        if (!int.TryParse(claimStr, out var userId))
        {
            throw new AccounteeUnauthorizedException();
        }

        return userId;
    }
}