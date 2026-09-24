using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface ICandidateProfileRepository
{
    Task<CandidateProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CandidateProfile profile,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CandidateProfile profile,
        CancellationToken cancellationToken = default);
}