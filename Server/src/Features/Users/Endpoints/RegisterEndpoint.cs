using Microsoft.Extensions.Primitives;
using src.Features.Shared.DTOs;
using src.Features.Shared.Endpoints;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;
using src.Features.Users.Interfaces;
using src.Utilities;

namespace src.Features.Users.Endpoints;

public class RegisterEndpoint(IHttpContextAccessor accessor) : BaseEndpoint<UserRequest, UserResponse, User>(accessor), IEndpoint
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

    private async Task<IResult> HandleAsync(
        UserRequest request,
        LinkGenerator generator,
        CancellationToken ct)
    {
        var mapper = GetRequired<IMapper<UserRequest, UserResponse, User>>();
        return await ExecuteAsync(
            request,
            action: async cancellationToken =>
            {
                bool usernameTaken = (await Repository.FindAsync(user => user.UserName == request.Username)).Any();
                bool emailTaken = (await Repository.FindAsync(user => user.Email == request.Email)).Any();
                if (usernameTaken)
                    return BadRequest("Username Is Already Taken!");
                if (emailTaken) 
                    return BadRequest("Email Is Already Taken!");
                
                User user = mapper.ToEntity(request);
                user.Id = Guid.NewGuid();
                user.Created = DateTime.UtcNow;
                user.Updated = DateTime.UtcNow;
                user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

                User? addedUser = await Repository.AddAsync(user);
                if (addedUser is null)
                    return BadRequest("Failed to register user. Try Again!");

                UserResponse response = mapper.ToResponse(addedUser!);
                
                response.Links.Add(
                    new Link()
                    {
                        Href = generator?.GetPathByRouteValues(
                            HttpContext,
                            routeName: "GetUserById",
                            values: new { id = response.Id}),
                        Rel = "self",
                        Type = string.Empty
                    });

                StringSegment etag = ComputeETag(response);
                SetETag(etag);

                return Ok(response);
            }, ct: ct);
    }
}