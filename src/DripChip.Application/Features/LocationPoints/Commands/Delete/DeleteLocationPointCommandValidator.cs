using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.LocationPoints.Commands.Delete;

public class DeleteLocationPointCommandValidator : AbstractValidator<DeleteLocationPointCommand>
{
    public DeleteLocationPointCommandValidator()
    {
        RuleFor(x => x.Id)
            .LocationPointId();
    }
}