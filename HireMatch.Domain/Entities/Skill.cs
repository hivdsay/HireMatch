namespace HireMatch.Domain.Entities;

public class Skill : Entity
{
    public string Name { get; private set; } = string.Empty;

    private Skill()
    {
    }

    public Skill(string name)
    {
        Name = name;
    }
    
}