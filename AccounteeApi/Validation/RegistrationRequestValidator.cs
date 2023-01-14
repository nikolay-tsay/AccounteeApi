using AccounteeService.Contracts.Requests;
using FluentValidation;

namespace AccounteeApi.Validation;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .NotEmpty(); 
        
        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

        RuleFor(x => x.PhoneNumber)
            .Matches("^\\s*(?:\\+?(\\d{1,3}))?([-. (]*(\\d{3})[-. )]*)?((\\d{3})[-. ]*(\\d{2,4})(?:[-.x ]*(\\d+))?)\\s*$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}