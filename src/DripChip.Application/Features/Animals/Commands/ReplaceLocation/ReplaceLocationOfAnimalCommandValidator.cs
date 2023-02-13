using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Commands.ReplaceLocation;

public class ReplaceLocationOfAnimalCommandValidator : AbstractValidator<ReplaceLocationOfAnimalCommand>
{
    public ReplaceLocationOfAnimalCommandValidator()
    {
        RuleFor(x => x.Id)
            .AnimalId();

        RuleFor(x => x.VisitedLocationPointId)
            .LocationPointId();

        RuleFor(x => x.LocationPointId)
            .LocationPointId();
    }
}