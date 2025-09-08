namespace src.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    
    public virtual User? User { get; init; }
    public virtual Post? Post { get; init; }
}