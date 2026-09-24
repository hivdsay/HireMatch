using HireMatch.Domain.Enums;

namespace HireMatch.Application.DTOs.Job;

public class CreateJobPostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public JobType JobType { get; set; }
    public WorkMode WorkMode { get; set; }
    public Guid CompanyId { get; set; }
    public List<string> RequiredSkills { get; set; } = new();
}