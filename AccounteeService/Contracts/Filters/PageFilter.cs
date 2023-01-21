using Microsoft.AspNetCore.Http;

namespace AccounteeService.Contracts.Filters;

public sealed record PageFilter(int PageSize, int PageNum)
{
    public static ValueTask<PageFilter?> BindAsync(HttpContext context)
    {
        var size = int.TryParse(context.Request.Query["pageSize"], out var pageSize)
            ? pageSize
            : 10;

        var num = int.TryParse(context.Request.Query["pageNum"], out var pageNum)
            ? pageNum
            : 1;
        
        var filter = new PageFilter(size, num);

        return ValueTask.FromResult<PageFilter?>(filter);
    }
}