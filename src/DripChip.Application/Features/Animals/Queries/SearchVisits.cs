using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Queries;

public static class SearchVisits
{
    public sealed record Query(
        long Id,
        DateTimeOffset? StartDateTime = default,
        DateTimeOffset? EndDateTime = default,
        int From = 0,
        int Size = 10) : IRequest<IEnumerable<Response>>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).AnimalId();
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
            var query =
                from visit in _context.AnimalLocationVisits
                where visit.VisitorId == request.Id
                where visit.DateTimeOfVisitLocationPoint >= request.StartDateTime || request.StartDateTime == null
                where visit.DateTimeOfVisitLocationPoint <= request.EndDateTime || request.EndDateTime == null
                select visit;

            return await query
                .OrderBy(x => x.Id)
                .Skip(request.From)
                .Take(request.Size)
                .ProjectToType<Response>()
                .ToListAsync(cancellationToken);
        }
    }

    public sealed record Response(
        long Id,
        long LocationPointId,
        DateTimeOffset DateTimeOfVisitLocationPoint);
}