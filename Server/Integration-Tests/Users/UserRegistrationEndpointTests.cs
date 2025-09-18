using System.Net.Http.Json;
using src.Features.Users.DTOs;

namespace Integration_Tests.Users;

public class UserRegistrationEndpointTests(StudentBlogFixture app) : TestBase<StudentBlogFixture>
{
    [Fact]
    public async Task RegisterUser_When_RequestIsValid_Should_Return200_And_UserResponse_WithSelfLink()
    {
        // Arrange
        UserRequest request = new()
        {
            Username = $"user_name",
            Firstname = "Ada",
            Lastname = "Lovelace",
            Email = $"email@email.com",
            Password = "Str0ngP@ss!"
        };

        HttpClient httpClient = app.CreateClient();
        
        // Act
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        UserResponse? body = await response.Content.ReadFromJsonAsync<UserResponse>();
        body.Should().NotBeNull();
        body.Username.Should().Be(request.Username);
        body.Firstname.Should().Be(request.Firstname);
        body.Lastname.Should().Be(request.Lastname);
        body.Email.Should().Be(request.Email);
        body.Id.Should().NotBeEmpty();
        body.Links.Should().NotBeNull();
        body.Links.Should().Contain(link =>
            (link.Href.Contains($"/api/user/{body.Id}") || link.Href.Contains("GetUserById")) 
            && link.Rel == "self");
    }
}