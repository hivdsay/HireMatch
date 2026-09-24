namespace HireMatch.Application.DTOs.Matching;

public class JobMatchResult
{
    public Guid JobPostId { get; set; }
    public Guid ResumeId { get; set; }

    public int MatchedSkills { get; set; }
    public int RequiredSkills { get; set; }

    public double MatchPercentage { get; set; }

    public List<string> MatchedSkillNames { get; set; } = new();
    public List<string> MissingSkillNames { get; set; } = new();
}