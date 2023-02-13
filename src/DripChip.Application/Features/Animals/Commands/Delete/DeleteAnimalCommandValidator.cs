using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Commands.Delete;

public class DeleteAnimalCommandValidator : AbstractValidator<DeleteAnimalCommand>
{
    public DeleteAnimalCommandValidator()
    {
        RuleFor(x => x.Id)
            .AnimalId();
    }
}