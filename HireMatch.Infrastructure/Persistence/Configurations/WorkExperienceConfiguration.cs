using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMatch.Infrastructure.Persistence.Configurations;

public class WorkExperienceConfiguration
    : IEntityTypeConfiguration<WorkExperience>
{
    public void Configure(EntityTypeBuilder<WorkExperience> builder)
    {
        builder.ToTable("work_experiences");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Position)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(3000);

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.IsCurrent)
            .IsRequired();

        builder.HasOne<CandidateProfile>()
            .WithMany()
            .HasForeignKey(x => x.CandidateProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CandidateProfileId);
    }
}