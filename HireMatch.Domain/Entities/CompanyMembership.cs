using HireMatch.Domain.Enums;

namespace HireMatch.Domain.Entities;

public class CompanyMembership : Entity
{
    public Guid UserId { get; private set; }

    public Guid CompanyId { get; private set; }

    public CompanyMemberRole Role { get; private set; }

    private CompanyMembership()
    {
    }

    public CompanyMembership(
        Guid userId,
        Guid companyId,
        CompanyMemberRole role)
    {
        UserId = userId;
        CompanyId = companyId;
        Role = role;
    }
    
}