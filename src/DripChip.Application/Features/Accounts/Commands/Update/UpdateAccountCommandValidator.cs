using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Accounts.Commands.Update;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator(IPasswordValidator<UpdateAccountCommand> passwordValidator)
    {
        RuleFor(x => x.AccountId)
            .GreaterThan(0);
        
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