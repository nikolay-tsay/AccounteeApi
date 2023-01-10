using AccounteeDomain.Models;
using FluentValidation;

namespace AccounteeApi.Validation;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}