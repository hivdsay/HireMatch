using HireMatch.Application.DTOs.Matching;

namespace HireMatch.Application.Abstractions.Services;

public interface IJobMatchingService
{
    Task<JobMatchResult> MatchAsync(
        Guid resumeId,
        Guid jobPostId,
        CancellationToken cancellationToken = default);
}