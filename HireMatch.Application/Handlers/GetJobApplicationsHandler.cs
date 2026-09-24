using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.DTOs.Job;
using HireMatch.Application.Exceptions;

namespace HireMatch.Application.Handlers;

public class GetJobApplicationsHandler
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICompanyMembershipRepository _companyMembershipRepository;
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public GetJobApplicationsHandler(
        IJobPostRepository jobPostRepository,
        ICompanyMembershipRepository companyMembershipRepository,
        IJobApplicationRepository jobApplicationRepository)
    {
        _jobPostRepository = jobPostRepository;
        _companyMembershipRepository = companyMembershipRepository;
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<IReadOnlyCollection<JobApplicationListItemDto>> Handle(
        Guid userId,
        Guid jobPostId,
        CancellationToken cancellationToken)
    {
        var jobPost = await _jobPostRepository.GetByIdAsync(
            jobPostId,
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

        var applications =
            await _jobApplicationRepository.GetByJobPostAsync(
                jobPostId,
                cancellationToken);

        return applications
            .Select(application => new JobApplicationListItemDto
            {
                ApplicationId = application.Id,
                CandidateProfileId = application.CandidateProfileId,
                Status = application.Status,
                AppliedAt = application.CreatedAt,
                ReviewedAt = application.ReviewedAt
            })
            .ToList();
    }
}