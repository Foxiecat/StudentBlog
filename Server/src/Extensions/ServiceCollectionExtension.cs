using System.Reflection;
using src.Features.Shared.Interfaces;
using src.Features.Users;
using src.Utilities;

namespace src.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterMappers(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Types.Program.Assembly;

        List<Type> mapperTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && type.GetInterfaces()
                .Any(interfaces => interfaces.IsGenericType && interfaces.GetGenericTypeDefinition() == Types.IMapper))
            .ToList();

        foreach (Type mapperType in mapperTypes)
        {
            Type interfaceType = mapperType.GetInterfaces().First(i => i.GetGenericTypeDefinition() == Types.IMapper);
            services.AddScoped(interfaceType, mapperType);
        }
    }
    
    public static void RegisterRepositories(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Types.Program.Assembly;
    
        List<Type> reposTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == Types.IBaseRepository))
            .ToList();
    
        foreach (Type repoType in reposTypes)
        {
            Type interfaceType = repoType.GetInterfaces().First();
            services.AddScoped(interfaceType, repoType);
        }
    }
}