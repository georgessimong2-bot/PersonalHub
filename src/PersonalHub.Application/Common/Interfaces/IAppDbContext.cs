using Microsoft.EntityFrameworkCore;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Note> Notes { get; }
    DbSet<Goal> Goals { get; }
    DbSet<Fund> Funds { get; }
    DbSet<FundType> FundTypes { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}