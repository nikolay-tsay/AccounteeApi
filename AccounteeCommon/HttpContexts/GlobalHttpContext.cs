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
        var items = HttpContextAccessor.HttpContext.Items;
        if(!items.TryGetValue(ClaimNames.IgnoreCompanyFilter, out var companyId))
        {
            throw new AccounteeUnauthorizedException();
        }

        return (int)companyId;
    }
    
    public static void SetCompanyId(int id)
    {
        var items = HttpContextAccessor.HttpContext.Items;
        items[ClaimNames.CompanyId] = id;
    }
}