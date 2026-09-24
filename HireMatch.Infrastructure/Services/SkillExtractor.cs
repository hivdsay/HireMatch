using HireMatch.Application.Abstractions.Services;

namespace HireMatch.Infrastructure.Services;

public class SkillExtractor : ISkillExtractor
{
    private static readonly string[] KnownSkills =
    {
        "C#",
        ".NET",
        "ASP.NET Core",
        "Entity Framework",
        "EF Core",
        "Java",
        "Spring Boot",
        "Python",
        "JavaScript",
        "TypeScript",
        "React",
        "Vue.js",
        "Angular",
        "HTML",
        "CSS",
        "SQL",
        "PostgreSQL",
        "MySQL",
        "MongoDB",
        "Redis",
        "Docker",
        "Kubernetes",
        "AWS",
        "Azure",
        "Git",
        "GitHub",
        "RabbitMQ",
        "Kafka",
        "REST API",
        "REST",
        "Microservices",
        "TensorFlow",
        "Machine Learning",
        "Artificial Intelligence"
    };

    public IReadOnlyCollection<string> ExtractSkills(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Array.Empty<string>();
        }

        var normalizedText = text.ToLowerInvariant();

        return KnownSkills
            .Where(skill =>
                normalizedText.Contains(
                    skill.ToLowerInvariant()))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}