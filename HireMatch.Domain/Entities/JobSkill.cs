namespace HireMatch.Domain.Entities;

public class JobSkill : Entity
{
    public Guid JobPostId { get; private set; }

    public Guid SkillId { get; private set; }

    public bool IsRequired { get; private set; }

    private JobSkill()
    {
    }

    public JobSkill(
        Guid jobPostId,
        Guid skillId,
        bool isRequired)
    {
        JobPostId = jobPostId;
        SkillId = skillId;
        IsRequired = isRequired;
    }
    
}