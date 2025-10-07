using System.ComponentModel.DataAnnotations;
using src.Features.Comments;
using src.Features.Posts;

namespace src.Features.Users;

public readonly record struct UserId(Guid Value)
{
    public static UserId NewId => new(Guid.NewGuid());
    public static UserId Empty => new(Guid.Empty);
}

public class User
{
    [Key]
    public UserId Id { get; set; }
     
    [Length(3, 30, ErrorMessage = "Invalid: Username has to be between 3 and 30 characters")]
    public required string Username { get; set; }
    
    [Length(2, 50, ErrorMessage = "Invalid: Firstname has to be between 2 and 50 characters")]
    public required string Firstname { get; set; }
    
    [Length(2, 50, ErrorMessage = "Invalid: Lastname has to be between 2 and 50 characters")]
    public required string Lastname { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }
    public required byte[] HashedPassword { get; set; }
    public required DateTime Created { get; set; }
    public required DateTime Updated { get; set; }
    public required bool IsAdminUser { get; set; }
    
    // Navigation properties
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}