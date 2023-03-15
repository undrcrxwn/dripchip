using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Commands;

public static class AddType
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
            var query =
                from animal in _context.Animals
                    .Include(animal => animal.AnimalTypes)
                    .Include(animal => animal.VisitedLocations)
                where animal.Id == request.AnimalId
                join animalType in _context.AnimalTypes on request.AnimalTypeId equals animalType.Id
                select new { Animal = animal, AnimalType = animalType };

            var result =
                await query.FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException();

            if (!result.Animal.AnimalTypes.Contains(result.AnimalType))
            {
                result.Animal.AnimalTypes.Add(result.AnimalType);
                await _context.SaveChangesAsync(cancellationToken);
            }

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