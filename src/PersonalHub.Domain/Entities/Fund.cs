namespace PersonalHub.Domain.Entities;

public class Fund : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;


    public Guid FundTypeId { get; set; }

    public FundType FundType { get; set; } = null!;

}