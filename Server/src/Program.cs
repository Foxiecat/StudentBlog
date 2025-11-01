using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using src.Database;
using src.Extensions;
using src.Features.Users;
using src.Middleware;
using src.Utilities;


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

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<StudentBlogDbContext>();

builder.Host.UseSerilog((context, config) => 
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpsRedirection(options => options.HttpsPort = 8081);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using IServiceScope scope = app.Services.CreateScope();
    StudentBlogDbContext dbContext = scope.ServiceProvider.GetRequiredService<StudentBlogDbContext>();
    await dbContext.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    if (!await roleManager.RoleExistsAsync(Roles.Admin))
        await roleManager.CreateAsync(new Role(Roles.Admin));
    if (await roleManager.RoleExistsAsync(Roles.User))
        await roleManager.CreateAsync(new Role(Roles.User));
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