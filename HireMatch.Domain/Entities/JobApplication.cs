using HireMatch.Domain.Enums;

namespace HireMatch.Domain.Entities;

public class JobApplication : Entity
{
    public Guid CandidateProfileId { get; private set; }
    public Guid JobPostId { get; private set; }

    public ApplicationStatus Status { get; private set; }

    public DateTime? ReviewedAt { get; private set; }

    private JobApplication() { }

    public JobApplication(
        Guid candidateProfileId,
        Guid jobPostId)
    {
        CandidateProfileId = candidateProfileId;
        JobPostId = jobPostId;
        Status = ApplicationStatus.Applied;
    }

    public void Review()
    {
        Status = ApplicationStatus.Reviewing;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Shortlist()
    {
        Status = ApplicationStatus.Shortlisted;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = ApplicationStatus.Rejected;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Hire()
    {
        Status = ApplicationStatus.Hired;
        ReviewedAt = DateTime.UtcNow;
    }
}