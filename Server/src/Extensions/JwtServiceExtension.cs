using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using src.Middleware;
using src.Services;
using src.Services.Interfaces;
using src.Utilities;

namespace src.Extensions;

public static class JwtServiceExtension
{
    public static void AddJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<JwtMiddleware>();
        
        services.Configure<JwtOptions>(
            configuration.GetSection("JWT"));

        services.AddAuthentication(configureOptions =>
        {
            configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            byte[] secretInBytes = Encoding.UTF8.GetBytes(configuration["JWT:Key"]
                                                          ?? throw new NullReferenceException("Missing JWT:Key"));
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(secretInBytes),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],
            };
        });

        services.AddAuthorization();
    }
}