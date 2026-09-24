using HireMatch.Domain.Enums;

namespace HireMatch.Application.Commands.Job;

public record UpdateJobApplicationStatusCommand(
    Guid ApplicationId,
    ApplicationStatus Status);