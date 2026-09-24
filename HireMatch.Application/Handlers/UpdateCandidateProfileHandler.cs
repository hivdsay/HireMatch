using FluentValidation;
using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.DTOs.Candidate;
using HireMatch.Application.Exceptions;

namespace HireMatch.Application.Handlers;

public class UpdateCandidateProfileHandler
{
    private readonly ICandidateProfileRepository _candidateProfileRepository;
    private readonly IValidator<UpdateCandidateProfileRequest> _validator;

    public UpdateCandidateProfileHandler(
        ICandidateProfileRepository candidateProfileRepository,
        IValidator<UpdateCandidateProfileRequest> validator)
    {
        _candidateProfileRepository = candidateProfileRepository;
        _validator = validator;
    }

    public async Task Handle(
        Guid userId,
        UpdateCandidateProfileRequest request,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var profile = await _candidateProfileRepository.GetByUserIdAsync(
            userId,
            cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException(
                "Candidate profile not found.");
        }

        profile.UpdateProfile(
            request.Headline,
            request.Bio,
            request.YearsOfExperience);

        await _candidateProfileRepository.UpdateAsync(
            profile,
            cancellationToken);
    }
}