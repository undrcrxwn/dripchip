using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using DripChip.Domain.Enumerations;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Commands;

public static class Create
{
    public sealed record Command(
        IEnumerable<long> AnimalTypes,
        float Weight,
        float Length,
        float Height,
        string Gender,
        int ChipperId,
        long ChippingLocationId) : IRequest<Response>;
    
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.AnimalTypes).NotEmpty();
            RuleForEach(x => x.AnimalTypes).GreaterThan(0);
            RuleFor(x => x.Weight).GreaterThan(0);
            RuleFor(x => x.Length).GreaterThan(0);
            RuleFor(x => x.Height).GreaterThan(0);
            RuleFor(x => x.Gender).IsInEnum(typeof(AnimalGender));
            RuleFor(x => x.ChipperId).AccountId();
            RuleFor(x => x.ChippingLocationId).LocationPointId();
        }
    }
    
    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            // Animal types validation
            var animalTypes = await _context.AnimalTypes.Where(animalType =>
                    request.AnimalTypes.Contains(animalType.Id))
                .ToListAsync(cancellationToken);

            if (request.AnimalTypes.Count() > animalTypes.Count)
                throw new NotFoundException("Some of the specified animal types does not exist.");

            // Chipper ID validation
            var chipper =
                await _context.Accounts.FindAsync(request.ChipperId)
                ?? throw new NotFoundException(nameof(Account), nameof(request.ChipperId));

            // Chipping location point ID validation
            var chippingLocationPoint =
                await _context.LocationPoints.FindAsync(request.ChippingLocationId)
                ?? throw new NotFoundException(nameof(LocationPoint), request.ChippingLocationId);

            var entity = request.Adapt<Animal>();
            entity.AnimalTypes = animalTypes;
            entity.Chipper = chipper;
            entity.ChippingLocation = chippingLocationPoint;
            entity.ChippingDateTime = DateTimeOffset.UtcNow.Trim(TimeSpan.TicksPerMillisecond);
        
            await _context.Animals.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Adapt<Response>();
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