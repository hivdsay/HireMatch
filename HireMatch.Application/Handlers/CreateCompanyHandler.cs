using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Commands.Company;
using HireMatch.Domain.Entities;
using HireMatch.Domain.Enums;

namespace HireMatch.Application.Handlers;

public class CreateCompanyHandler
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyMembershipRepository _companyMembershipRepository;

    public CreateCompanyHandler(
        ICompanyRepository companyRepository,
        ICompanyMembershipRepository companyMembershipRepository)
    {
        _companyRepository = companyRepository;
        _companyMembershipRepository = companyMembershipRepository;
    }

    public async Task<Guid> Handle(
        Guid userId,
        CreateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var company = new Company(
            command.Name,
            command.Description,
            command.Website,
            command.Location);

        await _companyRepository.AddAsync(
            company,
            cancellationToken);
        var membership = new CompanyMembership(
            userId,
            company.Id,
            CompanyMemberRole.Owner);

        await _companyMembershipRepository.AddAsync(
            membership,
            cancellationToken);

        return company.Id;
    }
}