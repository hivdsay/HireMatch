using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.DTOs.Job;
using HireMatch.Application.Exceptions;

namespace HireMatch.Application.Handlers;

public class GetJobPostByIdHandler
{
    private readonly IJobPostRepository _jobPostRepository;

    public GetJobPostByIdHandler(
        IJobPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }

    public async Task<JobPostDetailDto> Handle(
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

        return new JobPostDetailDto
        {
            Id = jobPost.Id,
            Title = jobPost.Title,
            Description = jobPost.Description,
            Location = jobPost.Location,
            JobType = jobPost.JobType,
            WorkMode = jobPost.WorkMode,
            Source = jobPost.Source,
            CompanyId = jobPost.CompanyId,
            ExternalId = jobPost.ExternalId,
            ExternalUrl = jobPost.ExternalUrl,
            PublishedAt = jobPost.PublishedAt,
            ExpiresAt = jobPost.ExpiresAt,
            CreatedAt = jobPost.CreatedAt
        };
    }
}