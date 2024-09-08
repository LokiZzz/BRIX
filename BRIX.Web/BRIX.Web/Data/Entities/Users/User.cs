using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BRIX.Web.Data.Entities.Characters;

namespace BRIX.Web.Data.Entities.Users
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Персонажи пользователя.
        /// </summary>
        public HashSet<UserCharacter> Characters { get; set; } = [];
    }

    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.Characters).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
