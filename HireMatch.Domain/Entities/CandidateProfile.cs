namespace HireMatch.Domain.Entities;

public class CandidateProfile : Entity
{
    public Guid UserId { get; private set; }

    public string? Headline { get; private set; }

    public string? Bio { get; private set; }

    public int? YearsOfExperience { get; private set; }

    private CandidateProfile()
    {
    }

    public CandidateProfile(Guid userId)
    {
        UserId = userId;
    }

    public void UpdateProfile(
        string? headline,
        string? bio,
        int? yearsOfExperience)
    {
        Headline = headline;
        Bio = bio;
        YearsOfExperience = yearsOfExperience;
    }
    
}