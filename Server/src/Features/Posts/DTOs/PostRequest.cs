namespace src.Features.Posts.DTOs;

public record PostRequest
{
    public string? Title { get; set; }
    public string? Content { get; set; }
}   