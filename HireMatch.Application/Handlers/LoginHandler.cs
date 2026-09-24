using FluentValidation;
using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Abstractions.Services;
using HireMatch.Application.Commands.Auth;

namespace HireMatch.Application.Handlers;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IValidator<LoginCommand> _validator;

    public LoginHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IValidator<LoginCommand> validator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _validator = validator;
    }

    public async Task<string> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var user = await _userRepository.GetByEmailAsync(
            command.Email,
            cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException(
                "Invalid email or password.");
        }

        var passwordIsValid = _passwordHasher.Verify(
            command.Password,
            user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new InvalidOperationException(
                "Invalid email or password.");
        }

        var token = _jwtTokenService.GenerateToken(
            user.Id,
            user.Email,
            user.Role.ToString());

        return token;
    }
}