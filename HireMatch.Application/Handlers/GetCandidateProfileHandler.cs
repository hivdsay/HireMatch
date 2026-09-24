using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Domain.Entities;

namespace HireMatch.Application.Handlers;

public class GetCandidateProfileHandler
{
    private readonly ICandidateProfileRepository _candidateProfileRepository;

    public GetCandidateProfileHandler(
        ICandidateProfileRepository candidateProfileRepository)
    {
        _candidateProfileRepository = candidateProfileRepository;
    }

    public async Task<CandidateProfile?> Handle(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _candidateProfileRepository.GetByUserIdAsync(
            userId,
            cancellationToken);
    }
}