using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;

namespace src.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime LastActive { get; set; }
    
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}