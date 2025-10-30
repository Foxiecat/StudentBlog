using System.ComponentModel.DataAnnotations;
using src.Features.Comments;
using src.Features.Posts;
using src.Features.Users.Interfaces;

namespace src.Features.Users;

public readonly record struct UserId(Guid Value)
{
    public static UserId NewId => new(Guid.NewGuid());
    public static UserId Empty => new(Guid.Empty);
}

public class User
{
    [Required, Key]
    public UserId Id { get; set; }
    
    [Required]
    [MinLength(2, ErrorMessage = "Invalid Length: Needs to be at least 2 characters")]
    [MaxLength(30, ErrorMessage = "Invalid Length: Cannot exceed 30 characters")]
    public string Username { get; set; } = string.Empty;
    
    [Required,
     MinLength(2, ErrorMessage = "Invalid Length: Needs to be at least 2 characters"),
     MaxLength(50, ErrorMessage = "Invalid Length: Cannot exceed 50 characters")]
    public string Firstname { get; set; } = string.Empty;
    
    [Required,
     MinLength(2, ErrorMessage = "Invalid Length: Needs to be at least 2 characters"),
     MaxLength(50, ErrorMessage = "Invalid Length: Cannot exceed 50 characters")]
    public string? Lastname { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string? HashedPassword { get; set; }
    
    [Required]
    public DateTime Created { get; set; }
    
    [Required]
    public DateTime Updated { get; set; }

    [Required] public ICollection<Role> Roles { get; set; } = new List<Role>();
    
    
    // Navigation properties
    public ICollection<Post> Posts { get; init; } = new HashSet<Post>();
    public ICollection<Comment> Comments { get; init; } = new HashSet<Comment>();
}