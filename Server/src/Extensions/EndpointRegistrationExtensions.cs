using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using src.Features.Shared.Endpoints;
using src.Utilities;

namespace src.Extensions;

public static class EndpointRegistrationExtensions
{
    public static void RegisterEndpoints(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Types.Program.Assembly;
        
        ServiceDescriptor[] endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(Types.IEndpoint))
            .Select(type => ServiceDescriptor.Transient(Types.IEndpoint, type))
            .ToArray();
        
        services.TryAddEnumerable(endpointServiceDescriptors);
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.Configure(builder);
        }

        return app;
    }
}