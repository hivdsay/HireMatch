using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;
using HireMatch.Infrastructure.Services;
using Moq;

namespace HireMatch.Tests.Services;

public class JobMatchingServiceTests
{
    private readonly Mock<IResumeSkillRepository> _resumeSkillRepository;
    private readonly Mock<IJobSkillRepository> _jobSkillRepository;
    private readonly Mock<ISkillRepository> _skillRepository;

    private readonly JobMatchingService _service;

    public JobMatchingServiceTests()
    {
        _resumeSkillRepository =
            new Mock<IResumeSkillRepository>();

        _jobSkillRepository =
            new Mock<IJobSkillRepository>();

        _skillRepository =
            new Mock<ISkillRepository>();

        _service = new JobMatchingService(
            _resumeSkillRepository.Object,
            _jobSkillRepository.Object,
            _skillRepository.Object);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturn80Percent_WhenFourOfFiveSkillsMatch()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var jobPostId = Guid.NewGuid();

        var csharpSkill = new Skill("C#");
        var dotnetSkill = new Skill(".NET");
        var aspNetSkill = new Skill("ASP.NET Core");
        var postgreSqlSkill = new Skill("PostgreSQL");
        var dockerSkill = new Skill("Docker");

        var resumeSkills = new List<ResumeSkill>
        {
            new(resumeId, csharpSkill.Id),
            new(resumeId, dotnetSkill.Id),
            new(resumeId, aspNetSkill.Id),
            new(resumeId, postgreSqlSkill.Id)
        };

        var jobSkills = new List<JobSkill>
        {
            new(jobPostId, csharpSkill.Id, true),
            new(jobPostId, dotnetSkill.Id, true),
            new(jobPostId, aspNetSkill.Id, true),
            new(jobPostId, postgreSqlSkill.Id, true),
            new(jobPostId, dockerSkill.Id, true)
        };

        var skills = new List<Skill>
        {
            csharpSkill,
            dotnetSkill,
            aspNetSkill,
            postgreSqlSkill,
            dockerSkill
        };

        SetupRepositories(
            resumeId,
            jobPostId,
            resumeSkills,
            jobSkills,
            skills);

        // Act
        var result = await _service.MatchAsync(
            resumeId,
            jobPostId);

        // Assert
        Assert.Equal(resumeId, result.ResumeId);
        Assert.Equal(jobPostId, result.JobPostId);

        Assert.Equal(4, result.MatchedSkills);
        Assert.Equal(5, result.RequiredSkills);

        Assert.Equal(80, result.MatchPercentage);

        Assert.Contains(
            "C#",
            result.MatchedSkillNames);

        Assert.Contains(
            ".NET",
            result.MatchedSkillNames);

        Assert.Contains(
            "ASP.NET Core",
            result.MatchedSkillNames);

        Assert.Contains(
            "PostgreSQL",
            result.MatchedSkillNames);

        Assert.Contains(
            "Docker",
            result.MissingSkillNames);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnZeroPercent_WhenJobHasNoRequiredSkills()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var jobPostId = Guid.NewGuid();

        var resumeSkill = new Skill("C#");

        var resumeSkills = new List<ResumeSkill>
        {
            new(resumeId, resumeSkill.Id)
        };

        var jobSkills = new List<JobSkill>();

        var skills = new List<Skill>
        {
            resumeSkill
        };

        SetupRepositories(
            resumeId,
            jobPostId,
            resumeSkills,
            jobSkills,
            skills);

        // Act
        var result = await _service.MatchAsync(
            resumeId,
            jobPostId);

        // Assert
        Assert.Equal(0, result.RequiredSkills);
        Assert.Equal(0, result.MatchedSkills);
        Assert.Equal(0, result.MatchPercentage);

        Assert.Empty(result.MatchedSkillNames);
        Assert.Empty(result.MissingSkillNames);
    }

    private void SetupRepositories(
        Guid resumeId,
        Guid jobPostId,
        IReadOnlyCollection<ResumeSkill> resumeSkills,
        IReadOnlyCollection<JobSkill> jobSkills,
        IReadOnlyCollection<Skill> skills)
    {
        _resumeSkillRepository
            .Setup(x => x.GetByResumeIdAsync(
                resumeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(resumeSkills);

        _jobSkillRepository
            .Setup(x => x.GetByJobPostIdAsync(
                jobPostId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(jobSkills);

        _skillRepository
            .Setup(x => x.GetByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(skills);
    }
    
    [Fact]
    public async Task MatchAsync_ShouldReturnZeroPercent_WhenNoSkillsMatch()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var jobPostId = Guid.NewGuid();

        var csharpSkill = new Skill("C#");
        var dotnetSkill = new Skill(".NET");
        var pythonSkill = new Skill("Python");
        var dockerSkill = new Skill("Docker");

        var resumeSkills = new List<ResumeSkill>
        {
            new(resumeId, csharpSkill.Id),
            new(resumeId, dotnetSkill.Id)
        };

        var jobSkills = new List<JobSkill>
        {
            new(jobPostId, pythonSkill.Id, true),
            new(jobPostId, dockerSkill.Id, true)
        };

        var skills = new List<Skill>
        {
            csharpSkill,
            dotnetSkill,
            pythonSkill,
            dockerSkill
        };

        SetupRepositories(
            resumeId,
            jobPostId,
            resumeSkills,
            jobSkills,
            skills);

        // Act
        var result = await _service.MatchAsync(
            resumeId,
            jobPostId);

        // Assert
        Assert.Equal(0, result.MatchedSkills);
        Assert.Equal(2, result.RequiredSkills);
        Assert.Equal(0, result.MatchPercentage);

        Assert.Empty(result.MatchedSkillNames);

        Assert.Contains(
            "Python",
            result.MissingSkillNames);

        Assert.Contains(
            "Docker",
            result.MissingSkillNames);
    }
    
    [Fact]
    public async Task MatchAsync_ShouldMatchSkillsCaseInsensitively()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var jobPostId = Guid.NewGuid();

        var candidateSkill = new Skill("C#");
        var requiredSkill = new Skill("c#");

        var resumeSkills = new List<ResumeSkill>
        {
            new(resumeId, candidateSkill.Id)
        };

        var jobSkills = new List<JobSkill>
        {
            new(jobPostId, requiredSkill.Id, true)
        };

        var skills = new List<Skill>
        {
            candidateSkill,
            requiredSkill
        };

        SetupRepositories(
            resumeId,
            jobPostId,
            resumeSkills,
            jobSkills,
            skills);

        // Act
        var result = await _service.MatchAsync(
            resumeId,
            jobPostId);

        // Assert
        Assert.Equal(1, result.MatchedSkills);
        Assert.Equal(1, result.RequiredSkills);
        Assert.Equal(100, result.MatchPercentage);

        Assert.Single(result.MatchedSkillNames);
        Assert.Empty(result.MissingSkillNames);
    }
}