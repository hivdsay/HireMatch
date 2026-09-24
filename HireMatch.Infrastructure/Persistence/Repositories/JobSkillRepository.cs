using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class JobSkillRepository : IJobSkillRepository
{
    private readonly AppDbContext _context;

    public JobSkillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        JobSkill jobSkill,
        CancellationToken cancellationToken = default)
    {
        await _context.JobSkills.AddAsync(
            jobSkill,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobSkill>> GetByJobPostIdAsync(
        Guid jobPostId,
        CancellationToken cancellationToken = default)
    {
        return await _context.JobSkills
            .Where(x => x.JobPostId == jobPostId)
            .ToListAsync(cancellationToken);
    }
}