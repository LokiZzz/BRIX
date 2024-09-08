using BRIX.Web.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BRIX.Web.Data.Entities.Characters
{
    public class UserCharacter : EntityBase<Guid>
    {
        /// <summary>
        /// Персонаж, сериализованный в JSON.
        /// </summary>
        [Required]
        public string CharacterJsonData { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Владелец персонажа.
        /// </summary>
        [Required]
        public required User User { get; set; }
    }

    public class UserCharacterConfiguration : IEntityTypeConfiguration<UserCharacter>
    {
        public void Configure(EntityTypeBuilder<UserCharacter> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.Characters).HasForeignKey(x => x.UserId);
            builder.ToTable(nameof(UserCharacter), DbSchemes.Characters);
        }
    }
}
