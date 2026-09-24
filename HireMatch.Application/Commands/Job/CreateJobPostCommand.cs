using HireMatch.Domain.Enums;

namespace HireMatch.Application.Commands.Job;

public record CreateJobPostCommand(
    string Title,
    string Description,
    string Location,
    JobType JobType,
    WorkMode WorkMode,
    Guid CompanyId,
    List<string> RequiredSkills);