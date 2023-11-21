namespace InspectorGadget.DTOs;

public class RegisterRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    
    public string Telephone { get; set; } = null!;
}