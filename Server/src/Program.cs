using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using src.Database;
using src.Extensions;
using src.Middleware;


Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();
            
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
builder.Services.AddOpenApi();
builder.Services.RegisterRepositories();
builder.Services.RegisterMappers();
builder.Services.AddEndpoints();

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtServices(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<StudentBlogDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
        optionsBuilder => optionsBuilder.MigrationsAssembly("src"));
});

builder.Host.UseSerilog((context, config) => 
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpsRedirection(options => options.HttpsPort = 8081);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    StudentBlogDbContext application = app.Services.CreateScope().ServiceProvider.GetRequiredService<StudentBlogDbContext>();
    IEnumerable<string>? pendingMigrations = await application.Database.GetPendingMigrationsAsync();
    if (!pendingMigrations.Contains(null))
        await application.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseAuthentication();
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