using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class JobApplicationRepository
    : IJobApplicationRepository
{
    private readonly AppDbContext _context;

    public JobApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JobApplication?> GetByIdAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(
                x => x.Id == applicationId,
                cancellationToken);
    }

    public async Task<JobApplication?> GetByCandidateAndJobAsync(
        Guid candidateProfileId,
        Guid jobPostId,
        CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(
                x => x.CandidateProfileId == candidateProfileId &&
                     x.JobPostId == jobPostId,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobApplication>> GetByCandidateAsync(
        Guid candidateProfileId,
        CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(x => x.CandidateProfileId == candidateProfileId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobApplication>> GetByJobPostAsync(
        Guid jobPostId,
        CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(x => x.JobPostId == jobPostId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        JobApplication application,
        CancellationToken cancellationToken = default)
    {
        await _context.JobApplications.AddAsync(
            application,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        JobApplication application,
        CancellationToken cancellationToken = default)
    {
        _context.JobApplications.Update(application);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}