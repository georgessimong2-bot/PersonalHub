namespace PersonalHub.Web.Services.Auth;

public class CurrentUser
{
    public bool IsAuthenticated { get; set; }

    public string Email { get; set; } = string.Empty;
}