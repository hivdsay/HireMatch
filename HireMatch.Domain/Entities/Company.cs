namespace HireMatch.Domain.Entities;

public class Company : Entity
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? Website { get; private set; }

    public string? Location { get; private set; }

    private Company()
    {
    }

    public Company(
        string name,
        string? description = null,
        string? website = null,
        string? location = null)
    {
        Name = name;
        Description = description;
        Website = website;
        Location = location;
    }

    public void Update(
        string name,
        string? description,
        string? website,
        string? location)
    {
        Name = name;
        Description = description;
        Website = website;
        Location = location;
    }
    
}