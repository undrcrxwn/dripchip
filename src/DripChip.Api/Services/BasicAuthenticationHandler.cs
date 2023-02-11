using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DripChip.Api.Services;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private static readonly Regex AuthorizationHeaderRegex = new("Basic (.*)");

    private readonly Application.Abstractions.IAuthenticationService _authenticationService;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        Application.Abstractions.IAuthenticationService authenticationService)
        : base(options, logger, encoder, clock) =>
        _authenticationService = authenticationService;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Response.Headers.Add("WWW-Authenticate", "Basic");

        // Require authorization header
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Authorization header is missing.");

        // Get encoded authorization key
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var match = AuthorizationHeaderRegex.Match(authorizationHeader);
        if (!match.Success)
            return AuthenticateResult.Fail("Authorization code is not properly formatted.");

        // Decode authorization key
        var encodedAuthorizationKey = match.Groups.Values.Last().Value;
        var decodedAuthorizationKeyBytes = Convert.FromBase64String(encodedAuthorizationKey);
        var decodedAuthorizationKey = Encoding.UTF8.GetString(decodedAuthorizationKeyBytes);

        // Parse credentials
        var credentials = decodedAuthorizationKey.Split(':', 2);
        var email = credentials[0];
        var password = credentials[1];

        // Authenticate user
        var user = await _authenticationService.AuthenticateAsync(email, password);
        if (user is null)
            return AuthenticateResult.Fail("The specified credentials are invalid.");

        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer),
            new(ClaimTypes.Name, user.UserName!)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new GenericPrincipal(identity, roles: null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}