using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Common;
using DripChip.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence;

public class ApplicationDbContext :
    IdentityDbContext<Account, IdentityRole<int>, int>,
    IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}