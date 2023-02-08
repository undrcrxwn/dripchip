using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Accounts.Queries.GetById;

public class GetAccountByIdQueryValidator : AbstractValidator<GetAccountByIdQuery>
{
    public GetAccountByIdQueryValidator()
    {
        RuleFor(x => x.AccountId)
            .AccountId();
    }
}