namespace src.Features.Users.DTOs;

public sealed record UserRequest(
    string? Username,
    string? Firstname,
    string? Lastname,
    string? Email,
    string? Password
    );