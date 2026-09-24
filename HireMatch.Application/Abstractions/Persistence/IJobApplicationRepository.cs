using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task<JobApplication?> GetByCandidateAndJobAsync(
        Guid candidateProfileId,
        Guid jobPostId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<JobApplication>> GetByCandidateAsync(
        Guid candidateProfileId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<JobApplication>> GetByJobPostAsync(
        Guid jobPostId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        JobApplication application,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        JobApplication application,
        CancellationToken cancellationToken = default);
}