using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DripChip.Api.Filters;

/// <summary>
/// An authorization filter that guarantees a user provides either valid authentication credentials or
/// no credentials at all in case the specified endpoint requires user authentication.
/// <example>
/// Invalid credentials passed to the endpoint annotated with 'AllowAnonymous' will result in 401 Unauthorized status being returned.
/// </example>
/// </summary>
public class ApiAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Do not try to authenticate user who has not provided any authorization headers.
        if (context.HttpContext.Request.Headers.Authorization.Count == 0)
            return;

        if (!context.IsEffectivePolicy(this))
            return;

        var services = context.HttpContext.RequestServices;
        var policyEvaluator = services.GetRequiredService<IPolicyEvaluator>();
        var policyProvider = services.GetRequiredService<IAuthorizationPolicyProvider>();
        var policy = await policyProvider.GetDefaultPolicyAsync();

        // Try to authenticate and authorize user.
        var authenticateResult = await policyEvaluator.AuthenticateAsync(policy, context.HttpContext);
        var authorizeResult = await policyEvaluator.AuthorizeAsync(policy, authenticateResult, context.HttpContext, context);
        var scheme = authenticateResult.Ticket?.AuthenticationScheme!;

        if (authorizeResult.Forbidden)
            context.Result = new ForbidResult(scheme);
        else if (authorizeResult.Challenged)
            context.Result = new ChallengeResult(scheme);
    }
}