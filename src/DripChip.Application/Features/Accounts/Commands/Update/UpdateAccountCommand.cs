using MediatR;

namespace DripChip.Application.Features.Accounts.Commands.Update;

public record UpdateAccountCommand(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<UpdateAccountResponse>;