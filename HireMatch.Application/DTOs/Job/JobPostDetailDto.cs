using HireMatch.Domain.Enums;

namespace HireMatch.Application.DTOs.Job;

public class JobPostDetailDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public JobType JobType { get; set; }

    public WorkMode WorkMode { get; set; }

    public JobSource Source { get; set; }

    public Guid? CompanyId { get; set; }

    public string? ExternalId { get; set; }

    public string? ExternalUrl { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }
}