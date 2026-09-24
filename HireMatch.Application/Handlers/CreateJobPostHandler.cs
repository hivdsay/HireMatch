using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Commands.Job;
using HireMatch.Application.Exceptions;
using HireMatch.Domain.Entities;

namespace HireMatch.Application.Handlers;

public class CreateJobPostHandler
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IJobSkillRepository _jobSkillRepository;
    private readonly ICompanyMembershipRepository _companyMembershipRepository;

    public CreateJobPostHandler(
        IJobPostRepository jobPostRepository,
        ISkillRepository skillRepository,
        IJobSkillRepository jobSkillRepository,
        ICompanyMembershipRepository companyMembershipRepository)
    {
        _jobPostRepository = jobPostRepository;
        _skillRepository = skillRepository;
        _jobSkillRepository = jobSkillRepository;
        _companyMembershipRepository = companyMembershipRepository;
    }
    public async Task<Guid> Handle(
        Guid userId,
        CreateJobPostCommand command,
        CancellationToken cancellationToken)
    {
        var isMember = await _companyMembershipRepository.IsMemberAsync(
            userId,
            command.CompanyId,
            cancellationToken);

        if (!isMember)
        {
            throw new ForbiddenException(
                "You are not a member of this company.");
        }
        var jobPost = new JobPost(
            command.Title,
            command.Description,
            command.Location,
            command.JobType,
            command.WorkMode,
            command.CompanyId);

        await _jobPostRepository.AddAsync(
            jobPost,
            cancellationToken);

        foreach (var skillName in command.RequiredSkills)
        {
            var skill = await _skillRepository.GetByNameAsync(
                skillName,
                cancellationToken);

            if (skill is null)
            {
                skill = new Skill(skillName);

                await _skillRepository.AddAsync(
                    skill,
                    cancellationToken);
            }

            var jobSkill = new JobSkill(
                jobPost.Id,
                skill.Id,
                true);

            await _jobSkillRepository.AddAsync(
                jobSkill,
                cancellationToken);
        }

        return jobPost.Id;
    }
}