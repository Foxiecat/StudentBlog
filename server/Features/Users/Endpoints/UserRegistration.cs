using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using src.Entities;
using src.Features.Shared;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;

namespace src.Features.Users.Endpoints;

public class UserRegistration(
    IUserRepository userRepository,
    IMapper<UserRequest, UserResponse, User> userMapper,
    IPasswordHasher<User> passwordHasher,
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
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
        user.Created = DateTime.UtcNow;
        user.LastActive = DateTime.UtcNow;

        User addedUser = await userRepository.AddAsync(user);
        UserResponse response = userMapper.ToResponse(addedUser);

        await Send.OkAsync(response, ct);
    }
}