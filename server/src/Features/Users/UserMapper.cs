using src.Entities;
using src.Features.Shared.Interfaces;
using src.Features.Users.DTOs;

namespace src.Features.Users;

public class UserMapper : IMapper<UserRequest, UserResponse, User>
{
    public User ToEntity(UserRequest request) => new()
    {
        Username = request.Username,
        Firstname = request.Firstname,
        Lastname = request.Lastname,
        Email = request.Email
    };

    public UserResponse ToResponse(User entity) => new()
    {
        Id = entity.Id,
        Username = entity.Username,
        Firstname = entity.Firstname,
        Lastname = entity.Lastname,
        Email = entity.Email,
        Created = entity.Created,
        Updated = entity.Updated,
        LastActive = entity.LastActive
    };
}