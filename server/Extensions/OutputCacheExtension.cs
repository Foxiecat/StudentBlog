using System.Net;

namespace src.Extensions;

public static class OutputCacheExtension
{
    public static void AddOutputCacheBasics(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(option => option.Expire(TimeSpan.FromMinutes(5)));
            options.AddPolicy("AuthenticatedUserCachePolicy", option =>
                option.SetVaryByHeader("Authorization").Expire(TimeSpan.FromMinutes(5)));
            
            options.AddPolicy("NoCache", option => option.NoCache());
        });
    }
}