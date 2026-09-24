using HireMatch.Domain.Enums;

namespace HireMatch.Application.DTOs.Job;

public class JobPostFilterRequest
{
    public string? Search { get; set; }

    public string? Location { get; set; }

    public JobType? JobType { get; set; }

    public WorkMode? WorkMode { get; set; }
}