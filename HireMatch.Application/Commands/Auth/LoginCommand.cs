namespace HireMatch.Application.Commands.Auth;

public record LoginCommand(
    string Email,
    string Password);