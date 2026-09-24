using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface ISkillRepository
{
    Task<Skill?> GetByIdAsync(
        Guid skillId,
        CancellationToken cancellationToken = default);

    Task<Skill?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Skill>> GetByIdsAsync(
        IEnumerable<Guid> skillIds,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Skill skill,
        CancellationToken cancellationToken = default);
}