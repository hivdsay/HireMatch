using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMatch.Infrastructure.Persistence.Configurations;

public class JobPostConfiguration : IEntityTypeConfiguration<JobPost>
{
    public void Configure(EntityTypeBuilder<JobPost> builder)
    {
        builder.ToTable("job_posts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.JobType)
            .IsRequired();

        builder.Property(x => x.WorkMode)
            .IsRequired();

        builder.Property(x => x.Source)
            .IsRequired();

        builder.Property(x => x.ExternalId)
            .HasMaxLength(200);

        builder.Property(x => x.ExternalUrl)
            .HasMaxLength(1000);

        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => new
            {
                x.Source,
                x.ExternalId
            })
            .IsUnique()
            .HasFilter("\"ExternalId\" IS NOT NULL");
    }
}