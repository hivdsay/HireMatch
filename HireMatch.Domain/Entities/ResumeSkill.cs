namespace HireMatch.Domain.Entities;

public class ResumeSkill : Entity
{
    public Guid ResumeId { get; private set; }

    public Guid SkillId { get; private set; }

    public int YearsOfExperience { get; private set; }

    private ResumeSkill()
    {
    }

    public ResumeSkill(
        Guid resumeId,
        Guid skillId,
        int yearsOfExperience = 0)
    {
        ResumeId = resumeId;
        SkillId = skillId;
        YearsOfExperience = yearsOfExperience;
    }
    
}