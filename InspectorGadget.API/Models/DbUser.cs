namespace InspectorGadget.Models;

public sealed class DbUser
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public string TelephoneNumber { get; set; }

    public string Login { get; set; }

    public string PasswordHash { get; set; }

    public string Role { get; set; }

    public Client? Client { get; set; }

    public Employee? Employee { get; set; }
}