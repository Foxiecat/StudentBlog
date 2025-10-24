using src.Features.Users.Interfaces;

namespace src.Features.Users;

public class Role : IRole
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}