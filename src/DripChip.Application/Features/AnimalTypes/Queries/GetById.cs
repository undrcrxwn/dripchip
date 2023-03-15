using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;

namespace DripChip.Application.Features.AnimalTypes.Queries;

public static class GetById
{
    public sealed record Query(long Id) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator() => RuleFor(x => x.Id).AnimalTypeId();
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity =
                await _context.AnimalTypes.FindAsync(request.Id)
                ?? throw new NotFoundException(nameof(AnimalType), request.Id);

            return entity.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, string Type);
}