using System.Reflection;
using src.Features.Shared.Interfaces;
using src.Features.Users;

namespace src.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterMappers(this IServiceCollection services)
    {
        Assembly assembly = typeof(UserMapper).Assembly;
        
        List<Type> mapperTypes = assembly.GetTypes()
            .Where(type => type is {IsClass: true, IsAbstract: false} && type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,,>)))
            .ToList();

        foreach (Type mapperType in mapperTypes)
        {
            Type interfaceType = mapperType.GetInterfaces().First(i => i .GetGenericTypeDefinition() == typeof(IMapper<,,>));
            services.AddSingleton(interfaceType, mapperType);
        }
    }
    
    public static void RegisterRepositories(this IServiceCollection services)
    {
        Assembly assembly = typeof(UserMapper).Assembly;
        
        List<Type> repoTypes = assembly.GetTypes()
            .Where(type => type is {IsClass: true, IsAbstract: false} && type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
            .ToList();

        foreach (Type repoType in repoTypes)
        {
            Type interfaceType = repoType.GetInterfaces().First();
            services.AddScoped(interfaceType, repoType);
        }
    }
}