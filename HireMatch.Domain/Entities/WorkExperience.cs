namespace HireMatch.Domain.Entities;

public class WorkExperience : Entity
{
    public Guid CandidateProfileId { get; private set; }

    public string CompanyName { get; private set; } = string.Empty;

    public string Position { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public bool IsCurrent { get; private set; }

    private WorkExperience()
    {
    }

    public WorkExperience(
        Guid candidateProfileId,
        string companyName,
        string position,
        DateTime startDate,
        DateTime? endDate = null,
        string? description = null,
        bool isCurrent = false)
    {
        CandidateProfileId = candidateProfileId;
        CompanyName = companyName;
        Position = position;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        IsCurrent = isCurrent;
    }
    
}