using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Features.Posts;
using src.Features.Users;

namespace src.Features.Comments;

public readonly record struct CommentId(Guid Value)
{
    public static CommentId NewId => new(Guid.NewGuid());
    public static CommentId Empty => new(Guid.Empty);
}

public class Comment
{
    [Key]
    public CommentId Id { get; set; }
    
    [ForeignKey("PostId")]
    public PostId PostId { get; set; }

    [ForeignKey("UserId")]
    public UserId UserId { get; set; }
    
    public required string Content { get; set; }
    public required DateTime DateCommented { get; set; }
    
    // Navigation properties
    public virtual Post? Post { get; set; }
    public virtual User? User { get; set; }
}