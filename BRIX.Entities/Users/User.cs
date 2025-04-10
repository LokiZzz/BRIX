using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BRIX.GameService.Entities.Characters;

namespace BRIX.GameService.Entities.Users
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

        /// <summary>
        /// NPC ������������.
        /// </summary>
        public HashSet<NPC> NPCs { get; set; } = [];

        /// <summary>
        /// ������� ����������� ����������� �����.
        /// </summary>
        public HashSet<EmailConfirmationTries> EmailConfirmationTries { get; set; } = [];
    }

    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.Characters).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.EmailConfirmationTries).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
