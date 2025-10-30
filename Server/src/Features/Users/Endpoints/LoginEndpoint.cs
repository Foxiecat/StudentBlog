using src.Features.Shared.Endpoints;
using src.Features.Users.DTOs;
using src.Utilities;

namespace src.Features.Users.Endpoints;

public abstract record LoginRequest(string UsernameOrEmail, string Password);

public class LoginEndpoint(
    IHttpContextAccessor accessor, 
    LinkGenerator generator) : BaseEndpoint<LoginRequest, UserResponse>(accessor), IEndpoint
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
        throw new NotImplementedException();
    }
}