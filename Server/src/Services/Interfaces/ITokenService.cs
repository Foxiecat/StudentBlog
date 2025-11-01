using src.Features.Users;

namespace src.Services.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
    Task<(string? userId, IEnumerable<string>? roles)> ValidateAccessToken(string accessToken);
}