using AccounteeDomain.Models;
using FluentValidation;

namespace AccounteeApi.Validation;

public class CompanyDtoValidator : AbstractValidator<CompanyDto>
{
    public CompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Email)
            .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .Matches("^\\s*(?:\\+?(\\d{1,3}))?([-. (]*(\\d{3})[-. )]*)?((\\d{3})[-. ]*(\\d{2,4})(?:[-.x ]*(\\d+))?)\\s*$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}