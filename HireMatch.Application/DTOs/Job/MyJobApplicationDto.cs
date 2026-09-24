using HireMatch.Domain.Enums;

namespace HireMatch.Application.DTOs.Job;

public class MyJobApplicationDto
{
    public Guid ApplicationId { get; set; }

    public Guid JobPostId { get; set; }

    public ApplicationStatus Status { get; set; }

    public DateTime AppliedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }
}