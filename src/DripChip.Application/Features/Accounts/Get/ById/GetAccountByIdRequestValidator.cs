using FluentValidation;

namespace DripChip.Application.Features.Accounts.Get.ById;

public class GetAccountByIdRequestValidator : AbstractValidator<GetAccountByIdRequest>
{
    public GetAccountByIdRequestValidator() =>
        RuleFor(x => x.AccountId)
            .GreaterThan(0);
}