using Microsoft.AspNetCore.Http;

namespace AccounteeService.Contracts.Filters;

public record OrderFilter(string? PropertyName, bool? IsDescending)
{
    public static ValueTask<OrderFilter?> BindAsync(HttpContext context)
    {
        var propName = context.Request.Query["propertyName"];
        var isDescending = bool.TryParse(context.Request.Query["isDescending"], out var value)
            ? value
            : false;

        var filter = new OrderFilter(propName, isDescending);

        return ValueTask.FromResult<OrderFilter?>(filter);
    }
}