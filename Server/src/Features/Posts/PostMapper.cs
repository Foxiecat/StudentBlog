using src.Entities;
using src.Features.Posts.DTOs;
using src.Features.Shared.Interfaces;

namespace src.Features.Posts;

public class PostMapper : IMapper<PostRequest, PostResponse, Post>
{
    public Post ToEntity(PostRequest request) => new()
    {
        Title = request.Title,
        Content = request.Content,
    };

    public PostResponse ToResponse(Post entity) => new()
    {
        Id = entity.Id,
        UserId = entity.UserId,
        Title = entity.Title,
        Content = entity.Content,
        Created = entity.Created
    };
}