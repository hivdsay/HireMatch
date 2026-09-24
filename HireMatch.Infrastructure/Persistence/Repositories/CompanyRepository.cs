using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMatch.Infrastructure.Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(
                x => x.Id == companyId,
                cancellationToken);
    }

    public async Task AddAsync(
        Company company,
        CancellationToken cancellationToken = default)
    {
        await _context.Companies.AddAsync(
            company,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}