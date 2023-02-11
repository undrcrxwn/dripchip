using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Accounts.Commands.Create;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(IPasswordValidator<CreateAccountCommand> passwordValidator)
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .Apply(passwordValidator)
            .NotEmpty();
        
        RuleFor(x => x.FirstName)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}