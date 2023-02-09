using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.AnimalTypes.Commands.Delete;

public class DeleteAnimalTypeCommandValidator : AbstractValidator<DeleteAnimalTypeCommand>
{
    public DeleteAnimalTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .AnimalTypeId();
    }
}