using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Features.Comments;
using src.Features.Users;

namespace src.Features.Posts;

public class Post
{
    [Required, Key]
    public Guid Id { get; init; }
    
    [Required, ForeignKey(nameof(User.Id))]
    public Guid UserId { get; init; }
    
    [Required,
     MinLength(3, ErrorMessage = "Invalid Length: Needs to be at least 3 characters"),
     MaxLength(50, ErrorMessage = "Invalid Length: Cannot exceed 50 characters")]
    public string? Title { get; set; }
    
    [Required]
    public string? Content { get; set; }
    
    [Required]
    public DateTime DatePosted { get; set; }
    
    
    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}