using AccounteeDomain.Models;
using FluentValidation;

namespace AccounteeApi.Validation;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.AmountUnit)
            .NotNull();

        RuleFor(x => x.IdCategory)
            .NotNull();
    }
}