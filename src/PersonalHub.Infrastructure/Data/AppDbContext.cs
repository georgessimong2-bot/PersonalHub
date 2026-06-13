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

    public DbSet<FundType> FundTypes => Set<FundType>();
}