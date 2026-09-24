using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class CompanyMembershipRepository
    : ICompanyMembershipRepository
{
    private readonly AppDbContext _context;

    public CompanyMembershipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        CompanyMembership membership,
        CancellationToken cancellationToken = default)
    {
        await _context.CompanyMemberships.AddAsync(
            membership,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<bool> IsMemberAsync(
        Guid userId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CompanyMemberships
            .AnyAsync(
                x => x.UserId == userId &&
                     x.CompanyId == companyId,
                cancellationToken);
    }
}