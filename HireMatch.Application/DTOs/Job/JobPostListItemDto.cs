using HireMatch.Domain.Enums;

namespace HireMatch.Application.DTOs.Job;

public class JobPostListItemDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public JobType JobType { get; set; }

    public WorkMode WorkMode { get; set; }

    public JobSource Source { get; set; }

    public DateTime CreatedAt { get; set; }
}