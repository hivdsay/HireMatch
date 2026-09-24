using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.DTOs.Job;
using HireMatch.Application.Exceptions;

namespace HireMatch.Application.Handlers;

public class GetMyJobApplicationsHandler
{
    private readonly ICandidateProfileRepository _candidateProfileRepository;
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public GetMyJobApplicationsHandler(
        ICandidateProfileRepository candidateProfileRepository,
        IJobApplicationRepository jobApplicationRepository)
    {
        _candidateProfileRepository = candidateProfileRepository;
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<IReadOnlyCollection<MyJobApplicationDto>> Handle(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var candidateProfile =
            await _candidateProfileRepository.GetByUserIdAsync(
                userId,
                cancellationToken);

        if (candidateProfile is null)
        {
            throw new NotFoundException(
                "Candidate profile not found.");
        }

        var applications =
            await _jobApplicationRepository.GetByCandidateAsync(
                candidateProfile.Id,
                cancellationToken);

        return applications
            .Select(application => new MyJobApplicationDto
            {
                ApplicationId = application.Id,
                JobPostId = application.JobPostId,
                Status = application.Status,
                AppliedAt = application.CreatedAt,
                ReviewedAt = application.ReviewedAt
            })
            .ToList();
    }
}