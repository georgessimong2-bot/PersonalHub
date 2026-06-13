namespace PersonalHub.Application.Features.Funds.Common;

public class FundDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid FundTypeId { get; set; }

    public string FundTypeName { get; set; } = string.Empty;
}