using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.DTOs.Job;

namespace HireMatch.Application.Handlers;

public class GetJobPostsHandler
{
    private readonly IJobPostRepository _jobPostRepository;

    public GetJobPostsHandler(
        IJobPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }

    public async Task<IReadOnlyCollection<JobPostListItemDto>> Handle(
        JobPostFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var jobPosts = await _jobPostRepository.GetFilteredAsync(
            filter,
            cancellationToken);

        return jobPosts
            .Select(job => new JobPostListItemDto
            {
                Id = job.Id,
                Title = job.Title,
                Location = job.Location,
                JobType = job.JobType,
                WorkMode = job.WorkMode,
                Source = job.Source,
                CreatedAt = job.CreatedAt
            })
            .ToList();
    }
}