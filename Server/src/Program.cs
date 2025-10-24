using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using src.Database;
using src.Extensions;
using src.Middleware;

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

            builder.Services.RegisterRepositories();
            builder.Services.RegisterMappers();

            builder.Services.AddEndpoints();

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            
            //builder.Services.AddJwtServices(builder.Configuration);

            builder.Services.AddDbContext<StudentBlogDbContext>(options => 
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            builder.Host.UseSerilog((context, config) => 
                config.ReadFrom.Configuration(context.Configuration));

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandler();
            
            app.UseAuthorization();

            /*ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            RouteGroupBuilder versionedGroup = app
                .MapGroup("api/v{version:apiVersion}")
                .WithApiVersionSet(apiVersionSet);*/
            
            app.MapEndpoints();

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