using Microsoft.AspNetCore.Identity;

namespace BRIX.GameService.Entities.Users
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role : IdentityRole<Guid> { }
}
