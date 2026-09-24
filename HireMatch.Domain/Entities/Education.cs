namespace HireMatch.Domain.Entities;

public class Education : Entity
{
    public Guid CandidateProfileId { get; private set; }

    public string SchoolName { get; private set; } = string.Empty;

    public string Degree { get; private set; } = string.Empty;

    public string FieldOfStudy { get; private set; } = string.Empty;

    public DateTime? StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    private Education()
    {
    }

    public Education(
        Guid candidateProfileId,
        string schoolName,
        string degree,
        string fieldOfStudy,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        CandidateProfileId = candidateProfileId;
        SchoolName = schoolName;
        Degree = degree;
        FieldOfStudy = fieldOfStudy;
        StartDate = startDate;
        EndDate = endDate;
    }
    
}