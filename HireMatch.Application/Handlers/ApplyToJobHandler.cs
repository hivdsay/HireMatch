using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Commands.Job;
using HireMatch.Application.Exceptions;
using HireMatch.Domain.Entities;

namespace HireMatch.Application.Handlers;

public class ApplyToJobHandler
{
    private readonly ICandidateProfileRepository _candidateProfileRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public ApplyToJobHandler(
        ICandidateProfileRepository candidateProfileRepository,
        IJobPostRepository jobPostRepository,
        IJobApplicationRepository jobApplicationRepository)
    {
        _candidateProfileRepository = candidateProfileRepository;
        _jobPostRepository = jobPostRepository;
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<Guid> Handle(
        Guid userId,
        ApplyToJobCommand command,
        CancellationToken cancellationToken)
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

        var jobPost = await _jobPostRepository.GetByIdAsync(
            command.JobPostId,
            cancellationToken);

        if (jobPost is null)
        {
            throw new NotFoundException(
                "Job post not found.");
        }

        var existingApplication =
            await _jobApplicationRepository.GetByCandidateAndJobAsync(
                candidateProfile.Id,
                command.JobPostId,
                cancellationToken);

        if (existingApplication is not null)
        {
            throw new ConflictException(
                "You have already applied to this job.");
        }

        var application = new JobApplication(
            candidateProfile.Id,
            command.JobPostId);

        await _jobApplicationRepository.AddAsync(
            application,
            cancellationToken);

        return application.Id;
    }
}