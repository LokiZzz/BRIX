using Microsoft.AspNetCore.Identity;

namespace BRIX.Web.Data.Entities.Users
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role : IdentityRole<Guid> { }
}
