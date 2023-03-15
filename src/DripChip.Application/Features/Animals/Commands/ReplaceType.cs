using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Commands;

public static class ReplaceType
{
    public sealed record Command(long Id, long OldTypeId, long NewTypeId) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).AnimalId();
            RuleFor(x => x.OldTypeId).AnimalTypeId();
            RuleFor(x => x.NewTypeId).AnimalTypeId();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var query =
                from animal in _context.Animals.Include(animal => animal.AnimalTypes)
                join animalType in _context.AnimalTypes.AsNoTracking() on request.NewTypeId equals animalType.Id
                where animal.Id == request.Id
                select new { Animal = animal, NewAnimalType = animalType };

            var result =
                await query.FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException();

            if (result.Animal.AnimalTypes.Any(animalType => animalType.Id == result.NewAnimalType.Id))
                throw new AlreadyExistsException();

            var oldAnimalType =
                result.Animal.AnimalTypes.SingleOrDefault(animalType => animalType.Id == request.OldTypeId)
                ?? throw new NotFoundException();

            result.Animal.AnimalTypes.Remove(oldAnimalType);
            result.Animal.AnimalTypes.Add(result.NewAnimalType);

            await _context.SaveChangesAsync(cancellationToken);
            return result.Animal.Adapt<Response>();
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