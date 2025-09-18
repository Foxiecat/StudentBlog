using FastEndpoints;
using src.Features.Users.DTOs;

namespace src.Features.Users.Endpoints;

public record LoginRequest(string UsernameOrEmail, string Password);

public class LoginEndpoint(IUserRepository userRepository) : Endpoint<LoginRequest, UserResponse>
{
    public override void Configure()
    {
        Post("api/auth/login");
        Description(builder => builder.WithName("Login"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        
    }
}