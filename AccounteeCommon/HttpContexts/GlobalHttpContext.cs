using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AccounteeCommon.HttpContexts;

public static class GlobalHttpContext
{
    public static IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    public static bool GetIgnoreCompanyFilter()
    {
        var items = HttpContextAccessor.HttpContext.Items;
        if (items.TryGetValue(ClaimNames.IgnoreCompanyFilter, out var value))
        {
            return (bool?)value == true;
        }

        return false;
    }
    
    public static void SetIgnoreCompanyFilter(bool value)
    {
        var items = HttpContextAccessor.HttpContext.Items;
        items[ClaimNames.IgnoreCompanyFilter] = value;
    }
    
    public static int GetCompanyId()
    {
        var claimStr = HttpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimNames.CompanyId)?.Value;
        if (!int.TryParse(claimStr, out var companyId))
        {
            throw new AccounteeUnauthorizedException();
        }
        
        return companyId;
    }
    
    public static int GetCurrentUserId()
    {
        var claimStr = HttpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimNames.UserId)?.Value;
        if (!int.TryParse(claimStr, out var userId))
        {
            throw new AccounteeUnauthorizedException();
        }

        return userId;
    }
}