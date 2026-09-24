using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMatch.Infrastructure.Persistence.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable("educations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SchoolName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Degree)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.FieldOfStudy)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne<CandidateProfile>()
            .WithMany()
            .HasForeignKey(x => x.CandidateProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CandidateProfileId);
    }
}