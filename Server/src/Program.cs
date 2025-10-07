using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using src.Database;

namespace src;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();
        
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudentBlogDbContext>(options => 
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            builder.Host.UseSerilog((context, config) => 
                config.ReadFrom.Configuration(context.Configuration));

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                using (IServiceScope scope = app.Services.CreateScope())
                {
                    StudentBlogDbContext db = scope.ServiceProvider.GetRequiredService<StudentBlogDbContext>();
                    db.Database.EnsureCreated();
                }
                
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.Run();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Application Terminated Unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}