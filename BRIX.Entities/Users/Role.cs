using Microsoft.AspNetCore.Identity;

namespace BRIX.Entity.Users
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role : IdentityRole<Guid> { }
}
