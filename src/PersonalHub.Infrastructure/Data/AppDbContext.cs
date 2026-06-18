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
    }
}