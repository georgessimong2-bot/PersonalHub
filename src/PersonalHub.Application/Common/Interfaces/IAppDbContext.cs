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
    DbSet<InvestmentStrategy> InvestmentStrategies { get; }
    DbSet<SfdrClassification> SfdrClassifications { get; }
    DbSet<InstrumentType> InstrumentTypes { get; }
    DbSet<Instrument> Instruments { get; }
    DbSet<InstrumentPrice> InstrumentPrices { get; }
    DbSet<BenchmarkPrice> BenchmarkPrices { get; }
    DbSet<Portfolio> Portfolios { get; }
    DbSet<PortfolioHolding> PortfolioHoldings { get; }
    DbSet<ExchangeRate> ExchangeRates { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}