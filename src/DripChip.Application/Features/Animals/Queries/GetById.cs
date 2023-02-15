using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Queries;

public static class GetById
{
    public sealed record Query(long Id) : IRequest<Response>;
    
    private sealed class Validator : AbstractValidator<Query>
    {
        public Validator() => RuleFor(x => x.Id).AnimalId();
    }
    
    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Animals
                .Include(animal => animal.AnimalTypes)
                .Include(animal => animal.Visits)
                .FirstOrDefaultAsync(animal => animal.Id == request.Id, cancellationToken);

            if (entity is null)
                throw new NotFoundException();

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