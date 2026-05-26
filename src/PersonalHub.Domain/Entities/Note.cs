namespace PersonalHub.Domain.Entities;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public Note() { }

    public Note(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }
}