using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMatch.Infrastructure.Persistence.Configurations;

public class CandidateProfileConfiguration
    : IEntityTypeConfiguration<CandidateProfile>
{
    public void Configure(EntityTypeBuilder<CandidateProfile> builder)
    {
        builder.ToTable("candidate_profiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Headline)
            .HasMaxLength(200);

        builder.Property(x => x.Bio)
            .HasMaxLength(2000);

        builder.Property(x => x.YearsOfExperience);

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<CandidateProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}