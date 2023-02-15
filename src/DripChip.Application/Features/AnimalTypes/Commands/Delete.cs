using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.AnimalTypes.Commands;

public static class Delete
{
    public sealed record Command(long Id) : IRequest;

    private sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(x => x.Id).AnimalTypeId();
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var animalType =
                await _context.AnimalTypes
                    .Include(type => type.Animals)
                    .FirstOrDefaultAsync(type => type.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();
        
            if (animalType.Animals.Any())
                throw new ValidationException(nameof(request.Id), "The specified animal type is attached to one or more animals.");

            _context.AnimalTypes.Remove(animalType);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}