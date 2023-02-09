using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Accounts.Commands.Delete;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .AccountId();
    }
}