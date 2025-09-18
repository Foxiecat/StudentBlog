using Testcontainers.PostgreSql;
using Microsoft.EntityFrameworkCore;
using src;
using src.Database;

namespace Integration_Tests;

public class StudentBlogFixture : AppFixture<Program>
{
    private PostgreSqlContainer? _container = null;

    protected override async ValueTask PreSetupAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17")
            .WithDatabase("Db")
            .WithUsername("Db")
            .WithPassword("Db")
            .Build();

        await _container.StartAsync();

        string? connectionString = _container.GetConnectionString();

        ServiceCollection serviceCollection = new();
        serviceCollection.AddDbContext<StudentBlogDbContext>(opts => 
            opts.UseNpgsql(connectionString, options =>
            {
                options.EnableRetryOnFailure();
                options.MigrationsAssembly(typeof(src.Migrations.InitialCreate).Assembly.GetName().Name);
            }));

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();
        StudentBlogDbContext dbContext = scope.ServiceProvider.GetRequiredService<StudentBlogDbContext>();
        
        await dbContext.Database.MigrateAsync();
    }

    protected override async ValueTask TearDownAsync()
    {
        if (_container is not null)
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
            _container = null;
        }
    }
}