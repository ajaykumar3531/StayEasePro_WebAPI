using AuthGuardPro_Application.Repos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


public class CustomAuthorizeUserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // Constructor accepts IServiceScopeFactory to create a scope for resolving scoped services
    public CustomAuthorizeUserMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only check for authorization if the endpoint requires it (i.e., has the [Authorize] attribute)
        var endpoint = context.GetEndpoint();
        var authorizationPolicy = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>();

        if (authorizationPolicy != null)
        {
            // If the endpoint requires authorization, check if the Authorization header exists
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                // Create a scope to resolve IAuthService from the scoped provider
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // Resolve the scoped IAuthService instance
                    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                    try
                    {
                        // Call AuthorizeUser to process the token and extract claims
                        await authService.AuthorizeUser();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Handle unauthorized access (optional: log or rethrow)
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
            }
            else
            {
                // If there's no authorization header, return unauthorized for authorized endpoints
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        // Continue to the next middleware in the pipeline
        await _next(context);
    }
}
