namespace InspectorGadget.DTOs;

public class ChangePasswordRequest
{
    public string Login { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}