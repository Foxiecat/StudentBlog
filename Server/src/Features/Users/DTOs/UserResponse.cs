namespace src.Features.Users.DTOs;

public record UserResponse
{
    public Guid Id { get; init; }
    public string? Username { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public bool IsAdminUser { get; set; }
}
