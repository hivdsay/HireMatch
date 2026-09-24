namespace HireMatch.Application.Abstractions.Services;

public interface IJwtTokenService
{
    string GenerateToken(
        Guid userId,
        string email,
        string role);
}