using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Features.Comments;
using src.Features.Users;

namespace src.Features.Posts;

public readonly record struct PostId(Guid Value)
{
    public static PostId NewId => new(Guid.NewGuid());
    public static PostId Empty => new(Guid.Empty);
}

public class Post
{
    [Key]
    public PostId Id { get; set; }
    
    [ForeignKey("UserId")]
    public UserId UserId { get; set; }
    
    [Length(3, 50, ErrorMessage = "Invalid: Title must be between 3 and 50 characters long")]
    public required string Title { get; set; }
    
    public required string Content { get; set; }
    public required DateTime DatePosted { get; set; }
    
    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}