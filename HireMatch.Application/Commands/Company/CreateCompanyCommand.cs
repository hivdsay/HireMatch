namespace HireMatch.Application.Commands.Company;

public record CreateCompanyCommand(
    string Name,
    string? Description,
    string? Website,
    string? Location);