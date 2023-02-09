using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.LocationPoints.Queries.GetById;

public class GetLocationPointByIdQueryValidator : AbstractValidator<GetLocationPointByIdQuery>
{
    public GetLocationPointByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .LocationPointId();
    }
}