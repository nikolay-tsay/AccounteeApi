using Microsoft.AspNetCore.Http;

namespace AccounteeService.Contracts.Filters;

public sealed record OrderFilter(string? OrderBy, bool? IsDescending)
{
    public static ValueTask<OrderFilter?> BindAsync(HttpContext context)
    {
        var propName = context.Request.Query["orderBy"];
        var isDescending = bool.TryParse(context.Request.Query["isDescending"], out var value) && value;

        var filter = new OrderFilter(propName, isDescending);

        return ValueTask.FromResult<OrderFilter?>(filter);
    }
}