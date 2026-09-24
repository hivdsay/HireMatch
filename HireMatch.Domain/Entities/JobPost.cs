using HireMatch.Domain.Enums;

namespace HireMatch.Domain.Entities;

public class JobPost : Entity
{
    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Location { get; private set; } = string.Empty;

    public JobType JobType { get; private set; }

    public WorkMode WorkMode { get; private set; }

    public JobSource Source { get; private set; }

    public Guid? CompanyId { get; private set; }

    public string? ExternalId { get; private set; }

    public string? ExternalUrl { get; private set; }

    public DateTime? PublishedAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    private JobPost()
    {
    }

    // Internal job
    public JobPost(
        string title,
        string description,
        string location,
        JobType jobType,
        WorkMode workMode,
        Guid companyId)
    {
        Title = title;
        Description = description;
        Location = location;
        JobType = jobType;
        WorkMode = workMode;
        CompanyId = companyId;
        Source = JobSource.Internal;
    }

    // External job
    public JobPost(
        string title,
        string description,
        string location,
        JobType jobType,
        WorkMode workMode,
        JobSource source,
        string externalId,
        string externalUrl)
    {
        Title = title;
        Description = description;
        Location = location;
        JobType = jobType;
        WorkMode = workMode;
        Source = source;
        ExternalId = externalId;
        ExternalUrl = externalUrl;
    }
    
}