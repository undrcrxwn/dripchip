using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DripChip.Api;

public class ApiAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        if (context.HttpContext.Request.Headers.Authorization.Count == 0)
            return;

        if (!context.IsEffectivePolicy(this))
            return;

        var services = context.HttpContext.RequestServices;
        var policyEvaluator = services.GetRequiredService<IPolicyEvaluator>();
        var policyProvider = services.GetRequiredService<IAuthorizationPolicyProvider>();
        var policy = await policyProvider.GetDefaultPolicyAsync();
        
        var authenticateResult = await policyEvaluator.AuthenticateAsync(policy, context.HttpContext);
        var authorizeResult = await policyEvaluator.AuthorizeAsync(policy, authenticateResult, context.HttpContext, context);

        var originalResult = context.Result;
        var scheme = authenticateResult.Ticket?.AuthenticationScheme!;
        
        if (authorizeResult.Forbidden)
            context.Result = new ForbidResult(scheme);
        else if (authorizeResult.Challenged)
            context.Result = new ChallengeResult(scheme);
        
        // If the user has specified credentials and the credentials are invalid
        if (!authorizeResult.Succeeded)
            return;

        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
            context.Result = originalResult;
    }
}