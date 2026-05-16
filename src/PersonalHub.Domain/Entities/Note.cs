namespace PersonalHub.Domain.Entities;

public class Note
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    private Note() { } // EF Core

    public Note(string title, string content)
    {
        Id = Guid.NewGuid();
        Title = title;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }
}