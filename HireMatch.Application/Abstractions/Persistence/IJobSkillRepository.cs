using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface IJobSkillRepository
{
    Task AddAsync(
        JobSkill jobSkill,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<JobSkill>> GetByJobPostIdAsync(
        Guid jobPostId,
        CancellationToken cancellationToken = default);
}