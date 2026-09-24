using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Abstractions.Services;
using HireMatch.Application.DTOs.Matching;

namespace HireMatch.Infrastructure.Services;

public class JobMatchingService : IJobMatchingService
{
    private readonly IResumeSkillRepository _resumeSkillRepository;
    private readonly IJobSkillRepository _jobSkillRepository;
    private readonly ISkillRepository _skillRepository;

    public JobMatchingService(
        IResumeSkillRepository resumeSkillRepository,
        IJobSkillRepository jobSkillRepository,
        ISkillRepository skillRepository)
    {
        _resumeSkillRepository = resumeSkillRepository;
        _jobSkillRepository = jobSkillRepository;
        _skillRepository = skillRepository;
    }

   public async Task<JobMatchResult> MatchAsync(
    Guid resumeId,
    Guid jobPostId,
    CancellationToken cancellationToken = default)
{
    var resumeSkills =
        await _resumeSkillRepository.GetByResumeIdAsync(
            resumeId,
            cancellationToken);

    var jobSkills =
        await _jobSkillRepository.GetByJobPostIdAsync(
            jobPostId,
            cancellationToken);

    var skillIds = resumeSkills
        .Select(x => x.SkillId)
        .Concat(jobSkills.Select(x => x.SkillId))
        .Distinct()
        .ToList();

    var skills = await _skillRepository.GetByIdsAsync(
        skillIds,
        cancellationToken);

    var skillDictionary = skills.ToDictionary(
        x => x.Id,
        x => x.Name);

    var candidateSkillNames = resumeSkills
        .Where(x => skillDictionary.ContainsKey(x.SkillId))
        .Select(x => skillDictionary[x.SkillId])
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();

    var requiredSkillNames = jobSkills
        .Where(x => skillDictionary.ContainsKey(x.SkillId))
        .Select(x => skillDictionary[x.SkillId])
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();

    var candidateSkills = candidateSkillNames
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    var matchedSkills = requiredSkillNames
        .Where(candidateSkills.Contains)
        .ToList();

    var missingSkills = requiredSkillNames
        .Where(skill => !candidateSkills.Contains(skill))
        .ToList();

    var matchPercentage = requiredSkillNames.Count == 0
        ? 0
        : (double)matchedSkills.Count /
          requiredSkillNames.Count *
          100;

    return new JobMatchResult
    {
        JobPostId = jobPostId,
        ResumeId = resumeId,
        MatchedSkills = matchedSkills.Count,
        RequiredSkills = requiredSkillNames.Count,
        MatchPercentage = Math.Round(
            matchPercentage,
            2),
        MatchedSkillNames = matchedSkills,
        MissingSkillNames = missingSkills
    };
}
}