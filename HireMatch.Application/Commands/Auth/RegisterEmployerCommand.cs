namespace HireMatch.Application.Commands.Auth;

public record RegisterEmployerCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password);