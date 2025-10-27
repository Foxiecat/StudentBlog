using src.Services.Interfaces;

namespace src.Middleware;

public class JwtMiddleware(
    ITokenService tokenService, 
    ILogger<JwtMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (token is not null)
        {
            (string? userId, IEnumerable<string>? roles) = tokenService.ValidateAccessToken(token);
            logger.LogInformation("User: {UserId}, Roles: {roles}", userId, roles);

            context.Items["UserId"] = userId;
            context.Items["Roles"] = roles;
        }

        await next(context);
    }
}