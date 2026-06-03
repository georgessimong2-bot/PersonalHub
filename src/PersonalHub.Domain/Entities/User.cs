namespace PersonalHub.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = string.Empty;

    private User()
    {

    }

    public User(string email)
    {
        Email = email;
    }

    public void Update(string email)
    {
        Email = email;
    }
}




