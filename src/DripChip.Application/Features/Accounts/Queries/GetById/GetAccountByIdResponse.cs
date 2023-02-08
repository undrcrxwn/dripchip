namespace DripChip.Application.Features.Accounts.Queries.GetById;

public record GetAccountByIdResponse(int Id, string FirstName, string LastName, string Email);