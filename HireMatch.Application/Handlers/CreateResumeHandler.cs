using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.DTOs.Candidate;
using HireMatch.Application.Exceptions;
using HireMatch.Domain.Entities;

namespace HireMatch.Application.Handlers;

public class CreateResumeHandler
{
    private readonly IResumeRepository _resumeRepository;
    private readonly ICandidateProfileRepository _candidateProfileRepository;

    public CreateResumeHandler(
        IResumeRepository resumeRepository,
        ICandidateProfileRepository candidateProfileRepository)
    {
        _resumeRepository = resumeRepository;
        _candidateProfileRepository = candidateProfileRepository;
    }

    public async Task<Guid> Handle(
        Guid userId,
        CreateResumeRequest request,
        string? extractedText,
        CancellationToken cancellationToken)
    {
        var candidateProfile =
            await _candidateProfileRepository.GetByUserIdAsync(
                userId,
                cancellationToken);

        if (candidateProfile is null)
        {
            throw new NotFoundException(
                "Candidate profile not found.");
        }

        var resume = new Resume(
            candidateProfile.Id,
            request.FileName,
            request.FileUrl);

        if (!string.IsNullOrWhiteSpace(extractedText))
        {
            resume.SetExtractedText(extractedText);
        }

        await _resumeRepository.AddAsync(
            resume,
            cancellationToken);

        return resume.Id;
    }
}