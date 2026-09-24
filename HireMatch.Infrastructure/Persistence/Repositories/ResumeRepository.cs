using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class ResumeRepository : IResumeRepository
{
    private readonly AppDbContext _context;

    public ResumeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Resume resume,
        CancellationToken cancellationToken = default)
    {
        await _context.Resumes.AddAsync(
            resume,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<Resume?> GetByIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Resumes
            .FirstOrDefaultAsync(
                x => x.Id == resumeId,
                cancellationToken);
    }

    public async Task<Resume?> GetByCandidateProfileIdAsync(
        Guid candidateProfileId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Resumes
            .FirstOrDefaultAsync(
                x => x.CandidateProfileId == candidateProfileId,
                cancellationToken);
    }
    
    public async Task<Resume?> GetByIdAndUserIdAsync(
        Guid resumeId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Resumes
            .Where(x => x.Id == resumeId)
            .Where(x => x.CandidateProfileId == 
                        _context.CandidateProfiles
                            .Where(p => p.UserId == userId)
                            .Select(p => p.Id)
                            .FirstOrDefault())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Resume resume,
        CancellationToken cancellationToken = default)
    {
        _context.Resumes.Update(resume);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}