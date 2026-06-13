namespace PersonalHub.Web.Components.Shared;

public class RegisterResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public List<string> ValidationErrors { get; set; } = [];
}