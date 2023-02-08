using MediatR;

namespace DripChip.Application.Features.Accounts.Commands.Register;

public record RegisterAccountCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<RegisterAccountResponse>;