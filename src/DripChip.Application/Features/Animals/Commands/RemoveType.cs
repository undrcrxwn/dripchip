using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Animals.Commands;

public static class RemoveType
{
    public sealed record Command(long AnimalId, long AnimalTypeId) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.AnimalId).AnimalId();
            RuleFor(x => x.AnimalTypeId).AnimalTypeId();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var animal =
                await _context.Animals
                    .Include(animal => animal.AnimalTypes)
                    .FirstOrDefaultAsync(animal => animal.Id == request.AnimalId, cancellationToken)
                ?? throw new NotFoundException();

            var animalType =
                await _context.AnimalTypes.FindAsync(request.AnimalTypeId)
                ?? throw new NotFoundException();

            if (!animal.AnimalTypes.Contains(animalType))
                throw new NotFoundException();

            if (animal.AnimalTypes.Count == 1)
                throw new ValidationException(nameof(request.AnimalTypeId),
                    "The specified animal type is the only type attached to the animal.");

            animal.AnimalTypes.Remove(animalType);

            await _context.SaveChangesAsync(cancellationToken);
            return animal.Adapt<Response>();
        }
    }

    public sealed record Response(
        long Id,
        IEnumerable<long> AnimalTypes,
        float Weight,
        float Length,
        float Height,
        string Gender,
        string LifeStatus,
        DateTimeOffset ChippingDateTime,
        int ChipperId,
        long ChippingLocationId,
        IEnumerable<long> VisitedLocations,
        DateTimeOffset? DeathDateTime);
}