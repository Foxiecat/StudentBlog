using FastEndpoints;
using src.Entities;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;

namespace src.Features.Users.Endpoints;

public class UserRegistration(
    IUserRepository userRepository,
    IMapper<UserRequest, UserResponse, User> userMapper) : Endpoint<UserRequest, UserResponse>
{
    public override void Configure()
    {
        Post("user/register");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UserRequest request, CancellationToken cancellationToken)
    {
        
    }
}