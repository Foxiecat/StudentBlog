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
        Post("api/auth/register");
        Description(builder => builder.WithName("UserRegistration"));
        AllowAnonymous();
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
            linkHelper.CreateLink(
                HttpContext,
                endpointName: "GetUserById",
                relation: "self",
                method: "GET",
                values: new { UserId = response.Id}));

        await Send.OkAsync(response, ct);
    }
}