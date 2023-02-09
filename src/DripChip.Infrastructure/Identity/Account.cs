using DripChip.Application.Abstractions.Identity;
using DripChip.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity;

public class Account : IdentityUser<int>, IAccount
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}