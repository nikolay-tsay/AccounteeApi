using AccounteeDomain.Models;
using FluentValidation;

namespace AccounteeApi.Validation;

public class ServiceDtoValiator :AbstractValidator<ServiceDto>
{
    public ServiceDtoValiator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.TotalPrice)
            .NotNull();

        RuleFor(x => x.IdCategory)
            .NotNull();
    }
}