using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMatch.Infrastructure.Persistence.Configurations;

public class ResumeSkillConfiguration : IEntityTypeConfiguration<ResumeSkill>
{
    public void Configure(EntityTypeBuilder<ResumeSkill> builder)
    {
        builder.ToTable("resume_skills");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.YearsOfExperience)
            .IsRequired();

        builder.HasOne<Resume>()
            .WithMany()
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Skill>()
            .WithMany()
            .HasForeignKey(x => x.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
            {
                x.ResumeId,
                x.SkillId
            })
            .IsUnique();
    }
}