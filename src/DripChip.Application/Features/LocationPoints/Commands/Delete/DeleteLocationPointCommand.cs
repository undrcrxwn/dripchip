using MediatR;

namespace DripChip.Application.Features.LocationPoints.Commands.Delete;

public record DeleteLocationPointCommand(long Id) : IRequest;