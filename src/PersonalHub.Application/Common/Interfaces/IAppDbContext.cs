using Microsoft.EntityFrameworkCore;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Note> Notes { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}