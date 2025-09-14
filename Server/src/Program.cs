using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using src.Database;
using src.Extensions;
using src.Features.Shared;
using src.Middleware;

namespace src;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.RegisterRepositories();
        builder.Services.RegisterMappers();
        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<LinkHelper>();

        builder.Services
            .AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>();
        
        builder.Services.AddAuthenticationJwtBearer(secret =>
            secret.SigningKey = builder.Configuration["JWT:Key"]);
        
        builder.Services
            .AddAuthorization()
            .AddFastEndpoints()
            .AddResponseCaching();
        
        builder.Services.AddDbContext<StudentBlogDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        WebApplication app = builder.Build();
        
        app.UseResponseCaching()
           .UseFastEndpoints(config => 
           {
               config.Versioning.Prefix = "v"; 
           });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseExceptionHandler("/error");
        
        app.UseAuthentication()
           .UseAuthorization();

        app.Run();
    }
}