namespace PersonalHub.Application.Features.Funds.Common;

public class FundDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? LegalName { get; set; }

    public string? FundCode { get; set; }

    public string? DomicileCountry { get; set; }

    public string? BaseCurrency { get; set; }

    public DateTime? LaunchDate { get; set; }

    public bool IsActive { get; set; }

    public string? Description { get; set; }

    public Guid FundTypeId { get; set; }

    public string FundTypeName { get; set; } = string.Empty;

    public int SubFundCount { get; set; }
}