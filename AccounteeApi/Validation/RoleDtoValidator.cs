using AccounteeDomain.Models;
using FluentValidation;

namespace AccounteeApi.Validation;

public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}