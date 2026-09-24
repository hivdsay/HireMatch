using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext _context;

    public SkillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Skill?> GetByIdAsync(
        Guid skillId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Skills
            .FirstOrDefaultAsync(
                x => x.Id == skillId,
                cancellationToken);
    }

    public async Task<Skill?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _context.Skills
            .FirstOrDefaultAsync(
                x => x.Name == name,
                cancellationToken);
    }
    
    public async Task<IReadOnlyCollection<Skill>> GetByIdsAsync(
        IEnumerable<Guid> skillIds,
        CancellationToken cancellationToken = default)
    {
        return await _context.Skills
            .Where(x => skillIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Skill skill,
        CancellationToken cancellationToken = default)
    {
        await _context.Skills.AddAsync(
            skill,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}