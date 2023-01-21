using System.Linq.Expressions;
using AccounteeCommon.Exceptions;
using AccounteeDomain.Entities.Base;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Extensions;

public static class QueryExtension
{
    public static async Task<T> FirstOrNotFoundAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken)
    {
        var result = await query.FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            throw new AccounteeNotFoundException();
        }

        return result;
    }

    public static async Task<T> FirstOrNotFoundAsync<T>(this IEnumerable<T> query, CancellationToken cancellationToken)
    {
        var result = await Task.Run(query.FirstOrDefault, cancellationToken);

        if (result is null)
        {
            throw new AccounteeNotFoundException();
        }

        return result;
    }

    public static IQueryable<T> TrackIf<T>(this IQueryable<T> source, bool condition) where T: class
    {
        if (condition)
        {
            return source;
        }

        return source.AsNoTracking();
    }

    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> source, string? searchValue) where T : ISearchable
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return source;
        }
        
        return source.Where(x => x.SearchValue.Contains(searchValue));
    }
    
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, PageFilter filter, CancellationToken cancellationToken)
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
    
    public static IOrderedQueryable<T> FilterOrder<T>(this IQueryable<T> query, OrderFilter orderFilter)
    {
        var prop = orderFilter.OrderBy ?? "Id";
        var type = typeof(T);
        var propInfo = type.GetProperty(prop);

        if (propInfo is null)
        {
            throw new AccounteeException();
        }

        var typeParams = new []
        {
            Expression.Parameter(type, string.Empty)
        };
        
        var method = orderFilter.IsDescending == true
            ? "OrderByDescending" 
            : "OrderBy";
        
        var result = (IOrderedQueryable<T>)query.Provider.CreateQuery(
            Expression.Call(
                typeof(Queryable),
                method,
                new[] { type, propInfo.PropertyType },
                query.Expression,
                Expression.Lambda(Expression.Property(typeParams[0], propInfo), typeParams))
        );

        return result;
    }
        
    public static async Task<T?> FirstAllowNullAsync<T>(this IQueryable<T> query, bool allowNull, CancellationToken cancellationToken)
    {
        var result = await query.FirstOrDefaultAsync(cancellationToken);

        if (result is null && !allowNull)
        {
            throw new AccounteeNotFoundException();
        }

        return result;
    }
}