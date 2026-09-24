using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Company company,
        CancellationToken cancellationToken = default);
}