using AccounteeApi.Middleware;
using FluentValidation.Results;

namespace AccounteeApi.Extensions;

public static class ValidationExtension
{
    public static ErrorResponse ToResponse(this IEnumerable<ValidationFailure> failure)
    {
        return new ErrorResponse(failure.Select(x => x.ErrorMessage));
    }
}