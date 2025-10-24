using src.Features.Shared.Endpoints;
using src.Features.Users.DTOs;
using src.Utilities;

namespace src.Features.Users.Endpoints;

public class GetUserByIdEndpoint(HttpContextAccessor accessor) : BaseEndpoint<Guid, UserResponse>(accessor), IEndpoint
{
    public override void Configure(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{id:guid}", HandleAsync)
            .WithName("GetUserById")
            .WithTags(Tags.Users)
            .AllowAnonymous();
    }

    private async Task<IResult> HandleAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}