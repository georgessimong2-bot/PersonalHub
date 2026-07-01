using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;
using PersonalHub.Infrastructure.Identity;

namespace PersonalHub.Infrastructure.Data;

public class AppDbContext
    : IdentityDbContext<AppUser, IdentityRole, string>, IAppDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Fund> Funds => Set<Fund>();
    public DbSet<SubFund> SubFunds => Set<SubFund>();
    public DbSet<ShareClass> ShareClasses => Set<ShareClass>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Benchmark> Benchmarks => Set<Benchmark>();
    public DbSet<InvestmentStrategy> InvestmentStrategies => Set<InvestmentStrategy>();
    public DbSet<SfdrClassification> SfdrClassifications => Set<SfdrClassification>();
    public DbSet<FundType> FundTypes => Set<FundType>();
    public DbSet<InstrumentType> InstrumentTypes => Set<InstrumentType>();
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<InstrumentPrice> InstrumentPrices => Set<InstrumentPrice>();
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<PortfolioHolding> PortfolioHoldings => Set<PortfolioHolding>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Fund>()
            .HasMany(x => x.SubFunds)
            .WithOne(x => x.Fund)
            .HasForeignKey(x => x.FundId);

        builder.Entity<SubFund>()
            .HasMany(x => x.ShareClasses)
            .WithOne(x => x.SubFund)
            .HasForeignKey(x => x.SubFundId);

        builder.Entity<SubFund>()
            .HasOne(x => x.Benchmark)
            .WithMany(x => x.SubFunds)
            .HasForeignKey(x => x.BenchmarkId);

        builder.Entity<SubFund>()
            .HasOne(x => x.InvestmentStrategy)
            .WithMany()
            .HasForeignKey(x => x.InvestmentStrategyId);

        builder.Entity<SubFund>()
            .HasOne(x => x.SfdrClassification)
            .WithMany()
            .HasForeignKey(x => x.SfdrClassificationId);

        builder.Entity<ShareClass>()
            .HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId);

        builder.Entity<Instrument>()
            .HasOne(x => x.InstrumentType)
            .WithMany(x => x.Instruments)
            .HasForeignKey(x => x.InstrumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Instrument>()
            .HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Instrument>()
            .HasIndex(x => x.ISIN)
            .IsUnique();

        builder.Entity<InstrumentPrice>()
            .HasOne(x => x.Instrument)
            .WithMany(x => x.Prices)
            .HasForeignKey(x => x.InstrumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<InstrumentPrice>()
            .HasIndex(x => new { x.InstrumentId, x.PriceDate })
            .IsUnique();

        builder.Entity<Portfolio>()
            .HasOne(x => x.ShareClass)
            .WithMany(x => x.Portfolios)
            .HasForeignKey(x => x.ShareClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PortfolioHolding>()
            .HasOne(x => x.Portfolio)
            .WithMany(x => x.Holdings)
            .HasForeignKey(x => x.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PortfolioHolding>()
            .HasOne(x => x.Instrument)
            .WithMany()
            .HasForeignKey(x => x.InstrumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<PortfolioHolding>()
            .HasIndex(x => new { x.PortfolioId, x.InstrumentId })
            .IsUnique();
    }
}