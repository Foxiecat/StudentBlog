using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using src.Features.Users;
using src.Features.Users.Interfaces;
using src.Services.Interfaces;
using src.Utilities;

namespace src.Services;

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    public string CreateTokenAsync(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        ];

        foreach (IRole role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role.Name!));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key ?? throw new NoNullAllowedException("JWT Key cannot be null!")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = options.Value.Issuer,
            Audience = options.Value.Audience,
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = credentials
        };


        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public (string? userId, IEnumerable<string>? roles) ValidateAccessToken(string accessToken)
    {
        throw new NotImplementedException();
    }
}