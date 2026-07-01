namespace PersonalHub.Domain.Entities;

public class Portfolio : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid ShareClassId { get; set; }

    public DateTime ValuationDate { get; set; }

    public bool IsActive { get; set; } = true;

    public ShareClass ShareClass { get; set; } = null!;

    public ICollection<PortfolioHolding> Holdings { get; set; } = [];
}
