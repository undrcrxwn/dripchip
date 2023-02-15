using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Extensions;
using DripChip.Domain.Enumerations;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Queries;

public static class Search
{
    public sealed record Query(
        DateTimeOffset? StartDateTime = default,
        DateTimeOffset? EndDateTime = default,
        int? ChipperId = default,
        long? ChippingLocationId = default,
        string? LifeStatus = default,
        string? Gender = default,
        int From = 0,
        int Size = 10) : IRequest<IEnumerable<Response>>;

    private sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            When(x => x.ChipperId is not null, () =>
                RuleFor(x => x.ChipperId!.Value).AccountId());

            When(x => x.ChippingLocationId is not null, () =>
                RuleFor(x => x.ChippingLocationId!.Value).LocationPointId());

            When(x => x.LifeStatus is not null, () =>
                RuleFor(x => x.LifeStatus!).IsInEnum(typeof(AnimalLifeStatus)));

            When(x => x.Gender is not null, () =>
                RuleFor(x => x.Gender!).IsInEnum(typeof(AnimalGender)));

            RuleFor(x => x.From).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Size).GreaterThan(0);
        }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var animals = _context.Animals
                .Include(animal => animal.AnimalTypes)
                .Include(animal => animal.Visits)
                // Filtering
                .Where(x =>
                    request.StartDateTime == null ||
                    x.ChippingDateTime > request.StartDateTime)
                .Where(x =>
                    request.EndDateTime == null ||
                    x.ChippingDateTime < request.EndDateTime)
                .Where(x =>
                    request.ChipperId == null ||
                    x.Chipper.Id == request.ChipperId)
                .Where(x =>
                    request.ChippingLocationId == null ||
                    x.ChippingLocation.Id == request.ChippingLocationId)
                .Where(x =>
                    request.LifeStatus == null ||
                    string.Equals(x.LifeStatus.ToString(), request.LifeStatus,
                        StringComparison.OrdinalIgnoreCase))
                .Where(x =>
                    request.Gender == null ||
                    string.Equals(x.Gender.ToString(), request.Gender,
                        StringComparison.OrdinalIgnoreCase))
                // Pagination
                .OrderBy(x => x.Id)
                .Skip(request.From)
                .Take(request.Size);

            return await animals
                .ProjectToType<Response>()
                .ToListAsync(cancellationToken);
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