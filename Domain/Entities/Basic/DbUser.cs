using Domain.Common;

namespace Domain.Entities.Basic;

public class DbUser : BaseEntity
{
    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string SecretKey { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual Employee? Employee { get; set; }
}