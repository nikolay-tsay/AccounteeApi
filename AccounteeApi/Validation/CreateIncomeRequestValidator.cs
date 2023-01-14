using AccounteeService.Contracts.Requests;
using FluentValidation;

namespace AccounteeApi.Validation;

public class CreateIncomeRequestValidator : AbstractValidator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DateTime)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}