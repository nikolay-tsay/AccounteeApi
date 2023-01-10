using AccounteeApi.Extensions;
using FluentValidation;

namespace AccounteeApi.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private IValidator<T> Validator { get; }

    public ValidationFilter(IValidator<T> validator)
    {
        Validator = validator;
    }
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validatable = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;
        if (validatable is null)
        {
            return Results.BadRequest();
        }

        var validationResult = await Validator.ValidateAsync(validatable);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors.ToResponse());
        }

        return await next(context);
    }
}