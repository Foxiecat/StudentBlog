using FastEndpoints;
using src.Entities;
using src.Features.Shared;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;

namespace src.Features.Users.Endpoints;

/// <summary>
/// Represents an endpoint responsible for handling user registration functionality.
/// </summary>
/// <remarks>
/// This endpoint processes incoming requests to register a new user, validates the data,
/// hashes the password, assigns necessary properties, saves the user entity to the repository,
/// and generates a response containing user details and related links.
/// </remarks>
/// <example>
/// - HTTP Method: POST
/// - URL: /api/auth/register
/// - Security: No authentication required (AllowAnonymous attribute).
/// </example>
/// <typeparam name="TRequest">The type of the request payload, expected to be a <see cref="UserRequest"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response payload, which is a <see cref="UserResponse"/> containing user details.</typeparam>
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
        user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
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