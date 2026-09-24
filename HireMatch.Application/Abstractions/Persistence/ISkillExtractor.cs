namespace HireMatch.Application.Abstractions.Services;

public interface ISkillExtractor
{
    IReadOnlyCollection<string> ExtractSkills(string text);
}