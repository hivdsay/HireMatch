using HireMatch.Domain.Entities;

namespace HireMatch.Application.Abstractions.Persistence;

public interface ICompanyMembershipRepository
{
    Task AddAsync(
        CompanyMembership membership,
        CancellationToken cancellationToken = default);

    Task<bool> IsMemberAsync(
        Guid userId,
        Guid companyId,
        CancellationToken cancellationToken = default);
}