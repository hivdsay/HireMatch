using HireMatch.Domain.Enums;

namespace HireMatch.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public string PasswordHash { get; private set; } = string.Empty;

    private User()
    {
    }

    public User(
        string email,
        string firstName,
        string lastName,
        string passwordHash,
        UserRole role)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
    
}