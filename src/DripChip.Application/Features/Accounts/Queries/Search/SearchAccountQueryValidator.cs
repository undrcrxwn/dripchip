using FluentValidation;

namespace DripChip.Application.Features.Accounts.Queries.Search;

public class SearchAccountQueryValidator : AbstractValidator<SearchAccountQuery>
{
    public SearchAccountQueryValidator()
    {
        RuleFor(x => x.From)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Size)
            .GreaterThan(0);
    }
}