using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Infrastructure.Data.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        // Table
        builder.ToTable("Notes");

        // Primary key
        builder.HasKey(x => x.Id);

        // Title
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        // Content
        builder.Property(x => x.Content)
            .HasMaxLength(4000);

        // CreatedAt
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // UpdatedAt
        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);
    }
}