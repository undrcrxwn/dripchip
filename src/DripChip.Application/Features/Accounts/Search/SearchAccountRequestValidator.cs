using FluentValidation;

namespace DripChip.Application.Features.Accounts.Search;

public class SearchAccountRequestValidator : AbstractValidator<SearchAccountRequest>
{
    public SearchAccountRequestValidator()
    {
        RuleFor(x => x.From)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Size)
            .GreaterThan(0);
    }
}