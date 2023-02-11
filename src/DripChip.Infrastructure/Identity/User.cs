using DripChip.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity;

public class User : IdentityUser<int>, IUser { }