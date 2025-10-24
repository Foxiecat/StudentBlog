namespace src.Features.Shared.Endpoints;

public interface IEndpoint
{
    void Configure(IEndpointRouteBuilder app);
}