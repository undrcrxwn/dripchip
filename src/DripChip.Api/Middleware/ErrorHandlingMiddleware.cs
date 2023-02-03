using System.Net;
using System.Text.Json;

namespace DripChip.Api.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = (int)(error switch
            {
                InvalidOperationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            });

            var result = JsonSerializer.Serialize(new
            {
                error = error.Message,
                traceId = context.TraceIdentifier
            });
            
            await response.WriteAsync(result);
        }
    }
}