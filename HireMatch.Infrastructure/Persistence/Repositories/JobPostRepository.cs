using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.DTOs.Job;
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
    
    public async Task<IReadOnlyCollection<JobPost>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.JobPosts
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyCollection<JobPost>> GetFilteredAsync(
        JobPostFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.JobPosts
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = $"%{filter.Search}%";

            query = query.Where(x =>
                EF.Functions.ILike(x.Title, search) ||
                EF.Functions.ILike(x.Description, search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Location))
        {
            query = query.Where(x =>
                x.Location.Contains(filter.Location));
        }

        if (filter.JobType.HasValue)
        {
            query = query.Where(x =>
                x.JobType == filter.JobType.Value);
        }

        if (filter.WorkMode.HasValue)
        {
            query = query.Where(x =>
                x.WorkMode == filter.WorkMode.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
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