using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Accounts.Commands.Register;

public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator(IPasswordValidator<RegisterAccountCommand> passwordValidator)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .Apply(passwordValidator)
            .NotEmpty();
    }
}