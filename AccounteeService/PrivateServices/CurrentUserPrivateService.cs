using System.Globalization;
using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Contracts.Models;
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


    public async Task<CurrentUser> GetCurrentUser(bool tracking, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var user = await AccounteeContext.Users
            .TrackIf(tracking)
            .Include(x => x.Role)
            .Include(x => x.Company)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        var lng = GetSystemLanguage(user.UserLanguage);
        var currentUser = new CurrentUser(user,  new CultureInfo(lng));
        
        return currentUser;
    }

    public void CheckUserRights(UserEntity user,UserRights toCheck)
    {
        if (user.Role.IsAdmin)
        {
            return;
        }
        
        bool hasRight = toCheck switch
        {
            UserRights.CanCreateCompany => user.Role.CanCreateCompany,
            UserRights.CanEditCompany => user.Role.CanEditCompany,
            UserRights.CanDeleteCompany => user.Role.CanDeleteCompany,
            
            UserRights.CanReadUsers => user.Role.CanReadUsers,
            UserRights.CanRegisterUsers => user.Role.CanRegisterUsers,
            UserRights.CanDeleteUsers => user.Role.CanDeleteUsers,
            UserRights.CanEditUsers => user.Role.CanEditUsers,
            
            UserRights.CanReadRoles => user.Role.CanReadRoles,
            UserRights.CanCreateRoles => user.Role.CanCreateRoles,
            UserRights.CanEditRoles => user.Role.CanEditRoles,
            UserRights.CanDeleteRoles => user.Role.CanDeleteRoles,
            
            UserRights.CanReadOutlay => user.Role.CanReadOutlay,
            UserRights.CanCreateOutlay => user.Role.CanCreateOutlay,
            UserRights.CanEditOutlay => user.Role.CanEditOutlay,
            UserRights.CanDeleteOutlay => user.Role.CanDeleteOutlay,
            
            UserRights.CanReadProducts => user.Role.CanReadProducts,
            UserRights.CanCreateProducts => user.Role.CanCreateProducts,
            UserRights.CanEditProducts => user.Role.CanEditProducts,
            UserRights.CanDeleteProducts => user.Role.CanDeleteProducts,
            
            UserRights.CanReadServices => user.Role.CanReadServices,
            UserRights.CanCreateServices => user.Role.CanCreateServices,
            UserRights.CanEditServices => user.Role.CanEditServices,
            UserRights.CanDeleteServices => user.Role.CanDeleteServices,
            
            UserRights.CanReadCategories => user.Role.CanReadCategories,
            UserRights.CanCreateCategories => user.Role.CanCreateCategories,
            UserRights.CanEditCategories => user.Role.CanEditCategories,
            UserRights.CanDeleteCategories => user.Role.CanDeleteCategories,
            
            UserRights.CanUploadFiles => user.Role.CanUploadFiles,
            _ => throw new AccounteeUnreachableException(ResourceRetriever.Get(CultureInfo.CurrentCulture, 
                    nameof(Resources.UnreachableReached), toCheck))
        };

        if (!hasRight)
        {
            throw new AccounteeUnauthorizedException();
        }
    }

    public async Task CheckCurrentUserRights(UserRights toCheck, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser(false, cancellationToken);
        CheckUserRights(currentUser.User, toCheck);
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

    private string GetSystemLanguage(UserLanguages language)
    {
        var result = language switch
        {
            UserLanguages.English => SystemLanguages.En.ToEnumString(),
            UserLanguages.Russian => SystemLanguages.Ru.ToEnumString(),
            _ => throw new AccounteeUnreachableException()
        };

        return result;
    }
}