using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class CandidateProfileRepository : ICandidateProfileRepository
{
    private readonly AppDbContext _context;

    public CandidateProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CandidateProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        CandidateProfile profile,
        CancellationToken cancellationToken = default)
    {
        await _context.CandidateProfiles.AddAsync(
            profile,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(
        CandidateProfile profile,
        CancellationToken cancellationToken = default)
    {
        _context.CandidateProfiles.Update(profile);

        await _context.SaveChangesAsync(cancellationToken);
    }
}