using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface IResumeSkillRepository
{
    Task AddAsync(
        ResumeSkill resumeSkill,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ResumeSkill>> GetByResumeIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken = default);
}