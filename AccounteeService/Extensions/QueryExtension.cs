using AccounteeCommon.Exceptions;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Extensions;

public static class QueryExtension
{
    public static async Task<T> FirstOrNotFound<T>(this IQueryable<T> query, CancellationToken cancellationToken)
    {
        var result = await query.FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            throw new AccounteeNotFoundException();
        }

        return result;
    }

    public static async Task<IEnumerable<T>> GetPage<T>(this IQueryable<T> query, PageFilter filter, CancellationToken cancellationToken)
    {
        if (filter.PageNum > 1)
        {
            query = query.Skip(filter.PageNum * filter.PageSize);
        }

        var list = await query
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return list;
    }
    
    public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> source, PageFilter filter, CancellationToken cancellationToken)
    {
        var paged = new PagedList<T>
        {
            PageNum = filter.PageNum,
            PageSize = filter.PageSize,
            TotalCount = source.Count()
        };
        paged.TotalPages = (int) Math.Ceiling(paged.TotalCount / (double) paged.PageSize);
        
        paged.Items = await source
            .Skip(paged.PageNum > 1 
                ? paged.PageNum * paged.PageSize 
                : 0)
            .Take(paged.PageSize)
            .ToListAsync(cancellationToken);

        return paged;
    }
}