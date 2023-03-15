using DripChip.Application.Abstractions.Identity;

namespace DripChip.Application.Models.Identity;

public abstract record AuthenticationResult
{
    public record Success(IUser User) : AuthenticationResult;
    public record Failure(string Reason) : AuthenticationResult;
}