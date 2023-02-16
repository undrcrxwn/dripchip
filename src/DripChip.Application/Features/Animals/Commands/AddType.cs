using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;

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
            var animal =
                await _context.Animals.FindAsync(request.AnimalId)
                ?? throw new NotFoundException();

            var animalType =
                await _context.AnimalTypes.FindAsync(request.AnimalTypeId)
                ?? throw new NotFoundException();

            if (!animal.AnimalTypes.Contains(animalType))
                animal.AnimalTypes.Add(animalType);

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