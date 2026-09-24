using FluentValidation;
using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Abstractions.Services;
using HireMatch.Application.Commands.Auth;
using HireMatch.Application.Exceptions;
using HireMatch.Domain.Entities;
using HireMatch.Domain.Enums;

namespace HireMatch.Application.Handlers;

public class RegisterEmployerHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<RegisterEmployerCommand> _validator;

    public RegisterEmployerHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IValidator<RegisterEmployerCommand> validator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<Guid> Handle(
        RegisterEmployerCommand command,
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

        var passwordHash = _passwordHasher.Hash(
            command.Password);

        var user = new User(
            command.Email,
            command.FirstName,
            command.LastName,
            passwordHash,
            UserRole.Employer);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        return user.Id;
    }
}