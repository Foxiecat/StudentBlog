using FastEndpoints;
using src.Entities;
using src.Features.Shared;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;

namespace src.Features.Users.Endpoints;

public class GetUserByIdEndpoint(
    IUserRepository userRepository,
    IMapper<UserRequest, UserResponse, User> userMapper,
    ILogger<GetUserByIdEndpoint> logger,
    LinkHelper linkHelper) : EndpointWithoutRequest<UserResponse>
{
    public override void Configure()
    {
        Get("api/user/{UserId}");
        Description(builder => builder.WithName("GetUserById"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid userId = Route<Guid>("UserId");
        
        User? user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            logger.LogError("User with id {UserId} was not found", userId);
            await Send.NotFoundAsync(ct);
        }

        UserResponse response = userMapper.ToResponse(user);
        response.Links.Add(linkHelper.CreateLink(
            HttpContext, 
            endpointName: "GetUserById", 
            relation: "self",
            method: "GET",
            values: new { UserId = response.Id }));

        await Send.OkAsync(response, ct);
    }
}