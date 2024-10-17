using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BRIX.Entity.Characters;

namespace BRIX.Entity.Users
{
    /// <summary>
    /// ������������.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// ��������� ������������.
        /// </summary>
        public HashSet<PlayerCharacter> Characters { get; set; } = [];
    }

    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.Characters).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
