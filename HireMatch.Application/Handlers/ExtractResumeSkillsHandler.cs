using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Abstractions.Services;
using HireMatch.Application.Exceptions;
using HireMatch.Domain.Entities;

namespace HireMatch.Application.Handlers;

public class ExtractResumeSkillsHandler
{
    private readonly IResumeRepository _resumeRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IResumeSkillRepository _resumeSkillRepository;
    private readonly ISkillExtractor _skillExtractor;

    public ExtractResumeSkillsHandler(
        IResumeRepository resumeRepository,
        ISkillRepository skillRepository,
        IResumeSkillRepository resumeSkillRepository,
        ISkillExtractor skillExtractor)
    {
        _resumeRepository = resumeRepository;
        _skillRepository = skillRepository;
        _resumeSkillRepository = resumeSkillRepository;
        _skillExtractor = skillExtractor;
    }

    public async Task Handle(
        Guid resumeId,
        CancellationToken cancellationToken)
    {
        var resume = await _resumeRepository.GetByIdAsync(
            resumeId,
            cancellationToken);

        if (resume is null)
        {
            throw new NotFoundException(
                "Resume not found.");
        }

        if (string.IsNullOrWhiteSpace(resume.ExtractedText))
        {
            throw new InvalidOperationException(
                "Resume does not contain extracted text.");
        }

        var skills = _skillExtractor.ExtractSkills(
            resume.ExtractedText);

        foreach (var skillName in skills)
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

            var resumeSkill = new ResumeSkill(
                resume.Id,
                skill.Id);

            await _resumeSkillRepository.AddAsync(
                resumeSkill,
                cancellationToken);
        }
    }
}