using src.Features.Shared.Endpoints;
using src.Features.Users.DTOs;
using src.Features.Users.Interfaces;
using src.Services.Interfaces;
using src.Utilities;

namespace src.Features.Users.Endpoints;

public abstract record LoginRequest(string UsernameOrEmail, string Password);

public class LoginEndpoint(IHttpContextAccessor accessor) : BaseEndpoint<LoginRequest, UserResponse, User>(accessor), IEndpoint
{
    public override void Configure(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", HandleAsync)
            .WithName("Login")
            .AllowAnonymous()
            .WithTags(Tags.Auth);
    }

    private async Task<IResult> HandleAsync(LoginRequest request, HttpContext context, CancellationToken ct)
    {
        ITokenService tokenService = GetRequired<ITokenService>();
        return await ExecuteAsync(
            request,
            action: async cancellationToken =>
            {
                User? user = (await Repository.FindAsync(user =>
                    user.UserName == request.UsernameOrEmail ||
                    user.Email == request.UsernameOrEmail)).First();

                if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash))
                    return Unauthorized();

                string? token = await tokenService.CreateTokenAsync(user);
                
                return Ok();
            }, ct: ct);
    }
}