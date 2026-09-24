using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HireMatch.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HireMatch.Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _secretKey;

    public JwtTokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"]
                     ?? throw new InvalidOperationException(
                         "JWT secret key is not configured.");
    }

    public string GenerateToken(
        Guid userId,
        string email,
        string role)
    {
        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                userId.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Email,
                email),

            new Claim(
                ClaimTypes.Role,
                role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}