using DripChip.Api.DTOs;
using DripChip.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[Route("[controller]")]
public class AccountsController : Controller
{
    private readonly UserManager<ApplicationUser> _users;

    public AccountsController(UserManager<ApplicationUser> users)
    {
        _users = users;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountRequest request)
    {
        // Email uniqueness validation
        var user = await _users.FindByEmailAsync(request.Email);
        if (user is not null)
            return Conflict("User with the specified email already exists.");

        user = new ApplicationUser
        {
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        // Password validation via user manager
        var passwordValidationTasks = _users.PasswordValidators
            .Select(x => x.ValidateAsync(_users, user, request.Password))
            .ToArray();

        var passwordValidationResults = await Task.WhenAll(passwordValidationTasks);
        foreach (var error in passwordValidationResults.SelectMany(x => x.Errors))
            ModelState.AddModelError(nameof(request.Password), error.Description);
        
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        
        // User creation
        var result = await _users.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);
            var description = string.Join(Environment.NewLine, errors);
            return BadRequest($"User cannot be created.\n{description}");
        }

        var response = new RegisterAccountResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        return Ok(response);
    }
}