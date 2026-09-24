namespace HireMatch.Domain.Entities;

public class Resume : Entity
{
    public Guid CandidateProfileId { get; private set; }

    public string FileName { get; private set; } = string.Empty;

    public string FileUrl { get; private set; } = string.Empty;

    public string? ExtractedText { get; private set; }

    public DateTime? AnalyzedAt { get; private set; }

    private Resume()
    {
    }

    public Resume(
        Guid candidateProfileId,
        string fileName,
        string fileUrl)
    {
        CandidateProfileId = candidateProfileId;
        FileName = fileName;
        FileUrl = fileUrl;
    }

    public void SetExtractedText(string extractedText)
    {
        ExtractedText = extractedText;
    }

    public void MarkAsAnalyzed()
    {
        AnalyzedAt = DateTime.UtcNow;
    }
    
}