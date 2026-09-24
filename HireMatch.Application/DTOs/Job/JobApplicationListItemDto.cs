using HireMatch.Domain.Enums;

namespace HireMatch.Application.DTOs.Job;

public class JobApplicationListItemDto
{
    public Guid ApplicationId { get; set; }

    public Guid CandidateProfileId { get; set; }

    public ApplicationStatus Status { get; set; }

    public DateTime AppliedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }
}