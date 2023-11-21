using InspectorGadget.Models;

namespace InspectorGadget.DTOs;

public class AuthorizeRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}