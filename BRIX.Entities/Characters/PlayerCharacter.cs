using BRIX.GameService.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BRIX.GameService.Entities.Characters
{
    public class PlayerCharacter : EntityBase<Guid>
    {
        /// <summary>
        /// Персонаж, сериализованный в JSON.
        /// </summary>
        [Required]
        public string CharacterJsonData { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Владелец персонажа.
        /// </summary>
        [Required]
        public User User { get; set; } = default!;
    }

    public class UserCharacterConfiguration : IEntityTypeConfiguration<PlayerCharacter>
    {
        public void Configure(EntityTypeBuilder<PlayerCharacter> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.Characters).HasForeignKey(x => x.UserId);
            builder.ToTable(nameof(PlayerCharacter), DbSchemes.Characters);
            builder.Property(x => x.Created).HasDefaultValueSql("getutcdate()");
        }
    }
}
