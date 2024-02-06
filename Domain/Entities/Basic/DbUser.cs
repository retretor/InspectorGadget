using Domain.Common;

namespace Domain.Entities.Basic;

public class DbUser : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string TelephoneNumber { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; }
    public Client? Client { get; set; }
    public Employee? Employee { get; set; }

    public DbUser(int id, string firstName, string secondName, string telephoneNumber, string login, string passwordHash,
        string role)
    {
        Id = id;
        FirstName = firstName;
        SecondName = secondName;
        TelephoneNumber = telephoneNumber;
        Login = login;
        PasswordHash = passwordHash;
        Role = role;
    }

    public DbUser() { }
}