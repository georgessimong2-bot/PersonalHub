namespace PersonalHub.Application.Features.FundTypes.Common;

public class FundTypeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}