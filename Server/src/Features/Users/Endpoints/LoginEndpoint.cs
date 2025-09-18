using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using src.Entities;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;

namespace src.Features.Users.Endpoints;

public record LoginRequest(string UsernameOrEmail, string Password);

public class LoginEndpoint(
    IUserRepository userRepository,
    IMapper<UserRequest, UserResponse, User> map) : Endpoint<LoginRequest, UserResponse>
{
    public override void Configure()
    {
        Post("api/auth/login");
        Description(builder => builder.WithName("Login"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        User? user = (await userRepository.FindAsync(user =>
            user.Username == request.UsernameOrEmail || user.Email == request.UsernameOrEmail)).FirstOrDefault();

        if (user is null)
        {
            // TODO: Add Logger
            await Send.NoContentAsync(ct);
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash))
        {
            // TODO: Add Logger
            await Send.UnauthorizedAsync(ct);
        }

        // TODO: Add JWT functionality
        await Send.OkAsync(map.ToResponse(user), ct);
    }
}