using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMatch.Infrastructure.Persistence.Configurations;

public class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
{
    public void Configure(EntityTypeBuilder<JobSkill> builder)
    {
        builder.ToTable("job_skills");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsRequired)
            .IsRequired();

        builder.HasOne<JobPost>()
            .WithMany()
            .HasForeignKey(x => x.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Skill>()
            .WithMany()
            .HasForeignKey(x => x.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
            {
                x.JobPostId,
                x.SkillId
            })
            .IsUnique();
    }
}