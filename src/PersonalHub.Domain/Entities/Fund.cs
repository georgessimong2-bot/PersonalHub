namespace PersonalHub.Domain.Entities;

public class Fund : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? LegalName { get; set; }

    public string? FundCode { get; set; }

    public string? DomicileCountry { get; set; }

    public string? BaseCurrency { get; set; }

    public DateTime? LaunchDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string? Description { get; set; }

    public Guid FundTypeId { get; set; }

    public FundType FundType { get; set; } = null!;

    public ICollection<SubFund> SubFunds { get; set; }
        = new List<SubFund>();
}