using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}