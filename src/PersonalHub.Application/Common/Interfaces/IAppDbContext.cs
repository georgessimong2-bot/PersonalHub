using Microsoft.EntityFrameworkCore;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Note> Notes { get; }
    DbSet<Goal> Goals { get; }
    DbSet<Fund> Funds { get; }
    DbSet<SubFund> SubFunds { get; }
    DbSet<ShareClass> ShareClasses { get; }
    DbSet<FundType> FundTypes { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<Benchmark> Benchmarks { get; }
    DbSet<AssetClass> AssetClasses { get; }
    DbSet<SfdrClassification> SfdrClassifications { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}