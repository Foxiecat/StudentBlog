namespace src.Entities;

public abstract record HalDTO
{
    public List<Link> Links { get; set; } = [];
}

public record Link
{
    public required string? Href { get; set; }
    public required string Rel { get; set; }
    public string? Method { get; set; }
}