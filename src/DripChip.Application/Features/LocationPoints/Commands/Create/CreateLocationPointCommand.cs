using MediatR;

namespace DripChip.Application.Features.LocationPoints.Commands.Create;

public record CreateLocationPointCommand(double Latitude, double Longitude) : IRequest<CreateLocationPointResponse>;