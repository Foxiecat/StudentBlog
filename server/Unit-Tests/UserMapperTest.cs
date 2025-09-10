using src.Entities;
using src.Features.Users;
using src.Features.Users.DTOs;

namespace Unit_Tests;

public class UserMapperTest
{
    private readonly UserMapper _userMapper = new();
    
    [Fact]
    public void MapToResponse_When_UserEntityIsValid_Should_Return_UserResponse()
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Username = "user",
            Firstname = "Firstname",
            Lastname = "Lastname",
            Email = "email@email.com",
            PasswordHash = "øsladkfjsaøldfkjasødlfkjasdølfkjasødl",
            Created = new DateTime(2025, 01, 01, 9, 55, 00),
            Updated = new DateTime(2025, 01, 01, 9, 55, 00),
            LastActive = new DateTime(2025, 01, 01, 9, 55, 00),
        };

        UserResponse response = _userMapper.ToResponse(user);
        
        Assert.NotNull(response);
        Assert.Equal(user.Id, response.Id);
        Assert.Equal(user.Username, response.Username);
        Assert.Equal(user.Firstname, response.Firstname);
        Assert.Equal(user.Lastname, response.Lastname);
        Assert.Equal(user.Email, response.Email);
        Assert.Equal(user.Created, response.Created);
        Assert.Equal(user.Updated, response.Updated);
        Assert.Equal(user.LastActive, response.LastActive);
    }

    [Fact]
    public void MapToEntity_When_UserRequestIsValid_Should_Return_User()
    {
        UserRequest request = new()
        {
            Username = "user",
            Firstname = "Firstname",
            Lastname = "Lastname",
            Email = "email@email.com"
        };
        
        User user = _userMapper.ToEntity(request);
        
        Assert.NotNull(user);
        Assert.Equal(request.Username, user.Username);
        Assert.Equal(request.Firstname, user.Firstname);
        Assert.Equal(request.Lastname, user.Lastname);
        Assert.Equal(request.Email, user.Email);
    }
}