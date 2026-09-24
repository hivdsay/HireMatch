using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface IJobPostRepository
{
    Task<JobPost?> GetByIdAsync(
        Guid jobPostId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        JobPost jobPost,
        CancellationToken cancellationToken = default);

    Task<bool> BelongsToCompanyAsync(
        Guid jobPostId,
        Guid companyId,
        CancellationToken cancellationToken = default);
}