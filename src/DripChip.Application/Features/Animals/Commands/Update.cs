using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using DripChip.Domain.Enumerations;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Animals.Commands;

public static class Update
{
    public sealed record Command(
        long Id,
        float Weight,
        float Length,
        float Height,
        string Gender,
        string LifeStatus,
        int ChipperId,
        long ChippingLocationId) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).AnimalId();
            RuleFor(x => x.Weight).GreaterThan(0);
            RuleFor(x => x.Length).GreaterThan(0);
            RuleFor(x => x.Height).GreaterThan(0);
            RuleFor(x => x.Gender).IsInEnum(typeof(AnimalGender));
            RuleFor(x => x.LifeStatus).IsInEnum(typeof(AnimalLifeStatus));
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
            var animal =
                await _context.Animals
                    .AsNoTracking()
                    .FirstOrDefaultAsync(animal => animal.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            var modifiedAnimal = request.Adapt<Animal>();

            if (animal.LifeStatus == AnimalLifeStatus.Dead && modifiedAnimal.LifeStatus != AnimalLifeStatus.Dead)
                throw new ValidationException(nameof(request.LifeStatus), "The life status of a dead animal cannot be changed.");

            if (animal.Visits.FirstOrDefault()?.LocationPointId == request.ChippingLocationId)
                throw new ValidationException(nameof(request.ChippingLocationId),
                    "The specified chipping location point matches the animal's first visited location.");

            _context.Animals.Attach(modifiedAnimal);
            _context.Animals.Update(modifiedAnimal);

            await _context.SaveChangesAsync(cancellationToken);
            return animal.Adapt<Response>();
        }
    }
    
    public sealed record Response(
        long Id,
        float Weight,
        float Length,
        float Height,
        string Gender,
        string LifeStatus,
        int ChipperId,
        long ChippingLocationId);
}