using FluentValidation;
using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Abstractions.Services;
using HireMatch.Application.Commands.Auth;
using HireMatch.Application.Exceptions;
using HireMatch.Domain.Entities;
using HireMatch.Domain.Enums;

namespace HireMatch.Application.Handlers;

public class RegisterCandidateHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ICandidateProfileRepository _candidateProfileRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<RegisterCandidateCommand> _validator;

    public RegisterCandidateHandler(
        IUserRepository userRepository,
        ICandidateProfileRepository candidateProfileRepository,
        IPasswordHasher passwordHasher,
        IValidator<RegisterCandidateCommand> validator)
    {
        _userRepository = userRepository;
        _candidateProfileRepository = candidateProfileRepository;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<Guid> Handle(
        RegisterCandidateCommand command,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var existingUser = await _userRepository.GetByEmailAsync(
            command.Email,
            cancellationToken);

        if (existingUser is not null)
        {
            throw new ConflictException(
                "A user with this email already exists.");
        }

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = new User(
            command.Email,
            command.FirstName,
            command.LastName,
            passwordHash,
            UserRole.Candidate);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        var candidateProfile = new CandidateProfile(user.Id);

        await _candidateProfileRepository.AddAsync(
            candidateProfile,
            cancellationToken);

        return user.Id;
    }
}