using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class JobPostRepository : IJobPostRepository
{
    private readonly AppDbContext _context;

    public JobPostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JobPost?> GetByIdAsync(
        Guid jobPostId,
        CancellationToken cancellationToken = default)
    {
        return await _context.JobPosts
            .FirstOrDefaultAsync(
                x => x.Id == jobPostId,
                cancellationToken);
    }

    public async Task AddAsync(
        JobPost jobPost,
        CancellationToken cancellationToken = default)
    {
        await _context.JobPosts.AddAsync(
            jobPost,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
    
    public async Task<bool> BelongsToCompanyAsync(
        Guid jobPostId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _context.JobPosts
            .AnyAsync(
                x => x.Id == jobPostId &&
                     x.CompanyId == companyId,
                cancellationToken);
    }
}