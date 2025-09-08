using src.Entities;

namespace src.Features.Users.DTOs;

public record UserResponse : HalDTO
{
    public Guid Id { get; init; }
    public string Username { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime LastActive { get; set; }
}