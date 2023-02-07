namespace DripChip.Application.Features.Accounts.Register;

public class RegisterAccountResponse
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}