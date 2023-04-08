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
            var animal =
                await _context.Animals
                    .Include(animal => animal.AnimalTypes)
                    .FirstOrDefaultAsync(animal => animal.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            var newAnimalType =
                await _context.AnimalTypes.FindAsync(request.NewTypeId)
                ?? throw new NotFoundException();

            if (animal.AnimalTypes.Contains(newAnimalType))
                throw new AlreadyExistsException();

            var oldAnimalType = animal.AnimalTypes.SingleOrDefault(animalType => animalType.Id == request.OldTypeId);
            if (oldAnimalType is null)
                throw new NotFoundException();
        
            animal.AnimalTypes.Remove(oldAnimalType);
            animal.AnimalTypes.Add(newAnimalType);

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