using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.AnimalTypes.Queries.GetById;

public class GetAnimalTypeByIdQueryValidator : AbstractValidator<GetAnimalTypeByIdQuery>
{
    public GetAnimalTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .AnimalTypeId();
    }
}