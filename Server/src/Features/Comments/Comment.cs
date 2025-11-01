using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Features.Posts;
using src.Features.Users;

namespace src.Features.Comments;

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("PostId")]
    public Guid PostId { get; set; }

    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    
    public required string Content { get; set; }
    public required DateTime DateCommented { get; set; }
    
    // Navigation properties
    public virtual Post? Post { get; set; }
    public virtual User? User { get; set; }
}