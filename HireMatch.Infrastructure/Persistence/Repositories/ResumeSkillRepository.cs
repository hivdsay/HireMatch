using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class ResumeSkillRepository : IResumeSkillRepository
{
    private readonly AppDbContext _context;

    public ResumeSkillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ResumeSkill resumeSkill,
        CancellationToken cancellationToken = default)
    {
        await _context.ResumeSkills.AddAsync(
            resumeSkill,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<ResumeSkill>> GetByResumeIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ResumeSkills
            .Where(x => x.ResumeId == resumeId)
            .ToListAsync(cancellationToken);
    }
}