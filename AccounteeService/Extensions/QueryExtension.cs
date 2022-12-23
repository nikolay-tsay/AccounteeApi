using AccounteeCommon.Exceptions;
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
}