namespace src.Entities;

public class Post
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    
    public virtual User? User { get; init; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}