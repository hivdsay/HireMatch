namespace HireMatch.Application.Commands.Auth;

public record RegisterCandidateCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password);