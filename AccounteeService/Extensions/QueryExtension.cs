using System.Linq.Expressions;
using AccounteeCommon.Exceptions;
using AccounteeDomain.Entities.Base;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.Extensions;

public static class QueryExtension
{
    public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> path) where T : class
    {
        if (condition)
        {
            return source.Include(path);
        }
        else
        {
            return source;
        }
    }

    public static async Task<T> FirstOrNotFound<T>(this IQueryable<T> query, CancellationToken cancellationToken)
    {
        var result = await query.FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            throw new AccounteeNotFoundException();
        }

        return result;
    }
    
    public static T FirstOrNotFound<T>(this IEnumerable<T> query, Func<T, bool> predicate)
    {
        var result = query.FirstOrDefault(predicate);

        if (result == null)
        {
            throw new AccounteeNotFoundException();
        }

        return result;
    }
    
    public static T FirstOrNotFound<T>(this IEnumerable<T> query)
    {
        var result = query.FirstOrDefault();

        if (result == null)
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
    
    public static IOrderedQueryable<T> FilterOrder<T>(this IQueryable<T> query, OrderFilter orderFilter)
    {
        if (orderFilter.PropertyName == null)
        {
            return (IOrderedQueryable<T>)query;
        }
        
        var type = typeof(T);
        var propInfo = type.GetProperty(orderFilter.PropertyName);

        if (propInfo == null)
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
}