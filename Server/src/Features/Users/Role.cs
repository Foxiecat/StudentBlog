using Microsoft.AspNetCore.Identity;
using src.Features.Users.Interfaces;

namespace src.Features.Users;

public class Role() : IdentityRole<Guid>
{
    public sealed override Guid Id { get; set; }
    public sealed override string? Name { get; set; }

    public Role(string roleName) : this()
    {
        Id = Guid.NewGuid();
        Name = roleName;
    }
}