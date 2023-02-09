using MediatR;

namespace DripChip.Application.Features.LocationPoints.Commands.Update;

public record UpdateLocationPointCommand(long Id, double Latitude, double Longitude) : IRequest<UpdateLocationPointResponse>;