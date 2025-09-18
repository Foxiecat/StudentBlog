using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Features.Users;

namespace Integration_Tests;

public static class ExtensionMethod
{
    public static void AddMyDbContext(this IServiceCollection services)
    {
        ServiceDescriptor? descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<StudentBlogDbContext>));

        if(descriptor is not null)
            services.Remove(descriptor);

        services.AddDbContext<StudentBlogDbContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"), b =>
                b.MigrationsAssembly(typeof(UserMapper).Assembly.GetName().Name));
        });
    }
}