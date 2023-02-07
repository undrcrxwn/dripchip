using FluentValidation;

namespace DripChip.Application.Features.Accounts.Register;

public class RegisterAccountRequestValidator : AbstractValidator<RegisterAccountRequest>
{
    public RegisterAccountRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}