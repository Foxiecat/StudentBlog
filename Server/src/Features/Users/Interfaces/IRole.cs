namespace src.Features.Users.Interfaces;

public interface IRole
{
    Guid? Id { get; set; }
    string? Name { get; set; }
}