namespace HireMatch.Application.DTOs.Candidate;

public class UpdateCandidateProfileRequest
{
    public string? Headline { get; set; }
    public string? Bio { get; set; }
    public int? YearsOfExperience { get; set; }
}