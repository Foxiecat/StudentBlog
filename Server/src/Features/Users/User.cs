using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using src.Features.Comments;
using src.Features.Posts;

namespace src.Features.Users;

public class User : IdentityUser<Guid>
{
    [Required, Key]
    public override Guid Id { get; set; }
    
    [Required]
    [MinLength(2, ErrorMessage = "Invalid Length: Needs to be at least 2 characters")]
    [MaxLength(30, ErrorMessage = "Invalid Length: Cannot exceed 30 characters")]
    public override string UserName { get; set; } = string.Empty;
    
    [Required,
     MinLength(2, ErrorMessage = "Invalid Length: Needs to be at least 2 characters"),
     MaxLength(50, ErrorMessage = "Invalid Length: Cannot exceed 50 characters")]
    public string Firstname { get; set; } = string.Empty;
    
    [Required,
     MinLength(2, ErrorMessage = "Invalid Length: Needs to be at least 2 characters"),
     MaxLength(50, ErrorMessage = "Invalid Length: Cannot exceed 50 characters")]
    public string? Lastname { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public override string? Email { get; set; }
    
    [Required]
    public override string? PasswordHash { get; set; }
    
    [Required]
    public DateTime Created { get; set; }
    
    [Required]
    public DateTime Updated { get; set; }
    
    
    // Navigation properties
    public ICollection<Post> Posts { get; init; } = new HashSet<Post>();
    public ICollection<Comment> Comments { get; init; } = new HashSet<Comment>();
}