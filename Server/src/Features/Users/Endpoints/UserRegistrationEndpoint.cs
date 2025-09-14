using FastEndpoints;
using src.Entities;
using src.Features.Shared;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;

namespace src.Features.Users.Endpoints;

public class UserRegistrationEndpoint(
    IUserRepository userRepository,
    IMapper<UserRequest, UserResponse, User> userMapper,
    LinkHelper linkHelper) : Endpoint<UserRequest, UserResponse>
{
    public override void Configure()
    {
        Post("user/register");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UserRequest request, CancellationToken ct)
    {
        User user = userMapper.ToEntity(request);
        user.Id = Guid.NewGuid();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.Created = DateTime.UtcNow;
        user.LastActive = DateTime.UtcNow;

        User addedUser = await userRepository.AddAsync(user);
        UserResponse response = userMapper.ToResponse(addedUser);
        
        response.Links.Add(
            linkHelper.CreateLink(HttpContext, "UserRegistration", "self", "POST"));

        await Send.OkAsync(response, ct);
    }
}