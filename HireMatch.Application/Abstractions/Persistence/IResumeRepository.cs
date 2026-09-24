using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface IResumeRepository
{
    Task AddAsync(
        Resume resume,
        CancellationToken cancellationToken = default);

    Task<Resume?> GetByIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken = default);

    Task<Resume?> GetByCandidateProfileIdAsync(
        Guid candidateProfileId,
        CancellationToken cancellationToken = default);

    Task<Resume?> GetByIdAndUserIdAsync(
        Guid resumeId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Resume resume,
        CancellationToken cancellationToken = default);
}