using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using src.Features.Shared.Endpoints;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;
using src.Features.Users.Interfaces;
using src.Utilities;

namespace src.Features.Users.Endpoints.Auth;

public class RegisterEndpoint(IHttpContextAccessor accessor) : BaseEndpoint<UserRequest, UserResponse>(accessor), IEndpoint
{
    public override void Configure(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", HandleAsync)
            .WithName("Register")
            .AllowAnonymous()
            .WithTags(Tags.Auth);
    }

    protected override (bool IsValid, IDictionary<string, string[]> Errors) ValidateRequest(UserRequest request)
    {
        Dictionary<string, string[]> errors = [];
        if (string.IsNullOrWhiteSpace(request.Email)) errors["email"] = ["Email is required."];
        if (string.IsNullOrWhiteSpace(request.Password)) errors["password"] = ["Password is required"];
        return (errors.Count == 0, errors);
    }

    private async Task<IResult> HandleAsync(UserRequest request, CancellationToken ct)
    {
        return await ExecuteAsync(
            request,
            action: async cancellationToken =>
            {
                var mapper = GetRequired<IMapper<UserRequest, UserResponse, User>>();
                IUserRepository repository = GetRequired<IUserRepository>();

                User user = mapper.ToEntity(request);
                user.Id = Users.UserId.NewId;
                user.Created = DateTime.UtcNow;
                user.Updated = DateTime.UtcNow;
                user.IsAdminUser = false;
                user.HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

                User? addedUser = await repository.AddAsync(user);
                if (addedUser is null)
                    return BadRequest("Failed to register user");

                UserResponse response = mapper.ToResponse(addedUser!);

                StringSegment etag = ComputeETag(response);
                SetETag(etag);

                return Created($"/users/{response.Id}", response);
            }, ct: ct);
    }
}