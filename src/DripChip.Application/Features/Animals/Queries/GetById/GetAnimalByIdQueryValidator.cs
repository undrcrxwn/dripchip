using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Queries.GetById;

public class GetAnimalByIdQueryValidator : AbstractValidator<GetAnimalByIdQuery>
{
    public GetAnimalByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .AnimalId();
    }
}