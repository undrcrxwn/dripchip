using MediatR;

namespace DripChip.Application.Features.LocationPoints.Queries.GetById;

public record GetLocationPointByIdQuery(long Id) : IRequest<GetLocationPointByIdResponse>;