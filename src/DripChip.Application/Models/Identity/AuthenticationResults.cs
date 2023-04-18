using DripChip.Domain.Entities;

namespace DripChip.Application.Models.Identity;

public abstract record AuthenticationResult
{
    public record Success(Account User) : AuthenticationResult;
    public record Failure(string Reason) : AuthenticationResult;
}