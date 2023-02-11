using MediatR;

namespace DripChip.Application.Features.Accounts.Commands.Create;

public record CreateAccountCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<CreateAccountResponse>;