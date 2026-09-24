using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Commands.Job;
using HireMatch.Application.Exceptions;
using HireMatch.Domain.Enums;

namespace HireMatch.Application.Handlers;

public class UpdateJobApplicationStatusHandler
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICompanyMembershipRepository _companyMembershipRepository;

    public UpdateJobApplicationStatusHandler(
        IJobApplicationRepository jobApplicationRepository,
        IJobPostRepository jobPostRepository,
        ICompanyMembershipRepository companyMembershipRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _jobPostRepository = jobPostRepository;
        _companyMembershipRepository = companyMembershipRepository;
    }

    public async Task Handle(
        Guid userId,
        UpdateJobApplicationStatusCommand command,
        CancellationToken cancellationToken)
    {
        var application =
            await _jobApplicationRepository.GetByIdAsync(
                command.ApplicationId,
                cancellationToken);

        if (application is null)
        {
            throw new NotFoundException(
                "Job application not found.");
        }

        var jobPost = await _jobPostRepository.GetByIdAsync(
            application.JobPostId,
            cancellationToken);

        if (jobPost is null)
        {
            throw new NotFoundException(
                "Job post not found.");
        }

        if (jobPost.CompanyId is null)
        {
            throw new NotFoundException(
                "Company not found.");
        }

        var isMember =
            await _companyMembershipRepository.IsMemberAsync(
                userId,
                jobPost.CompanyId.Value,
                cancellationToken);

        if (!isMember)
        {
            throw new ForbiddenException(
                "You are not a member of this company.");
        }

        switch (command.Status)
        {
            case ApplicationStatus.Reviewing:
                application.Review();
                break;

            case ApplicationStatus.Shortlisted:
                application.Shortlist();
                break;

            case ApplicationStatus.Rejected:
                application.Reject();
                break;

            case ApplicationStatus.Hired:
                application.Hire();
                break;

            default:
                throw new InvalidOperationException(
                    "Invalid application status.");
        }

        await _jobApplicationRepository.UpdateAsync(
            application,
            cancellationToken);
    }
}