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

    public sealed class Validator : AbstractValidator<Query>
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
            AnimalLifeStatus? lifeStatus = request.LifeStatus is not null
                ? Enum.Parse<AnimalLifeStatus>(request.LifeStatus, ignoreCase: true)
                : null;

            AnimalGender? gender = request.Gender is not null
                ? Enum.Parse<AnimalGender>(request.Gender, ignoreCase: true)
                : null;

            var query =
                from animal in _context.Animals
                    .Include(animal => animal.AnimalTypes)
                    .Include(animal => animal.VisitedLocations)
                where animal.ChippingDateTime >= request.StartDateTime || request.StartDateTime == null
                where animal.ChippingDateTime <= request.EndDateTime || request.EndDateTime == null
                where animal.ChipperId == request.ChipperId || request.ChipperId == null
                where animal.ChippingLocationId == request.ChippingLocationId || request.ChippingLocationId == null
                where animal.LifeStatus == lifeStatus || lifeStatus == null
                where animal.Gender == gender || gender == null
                select animal;

            return await query
                .OrderBy(animal => animal.Id)
                .Skip(request.From)
                .Take(request.Size)
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