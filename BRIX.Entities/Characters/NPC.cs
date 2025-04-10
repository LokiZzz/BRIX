using BRIX.GameService.Entities.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BRIX.GameService.Entities.Characters
{
    public class NPC : EntityBase<Guid>
    {
        /// <summary>
        /// Персонаж, сериализованный в JSON.
        /// </summary>
        [Required]
        public string NPCJsonData { get; set; } = string.Empty;

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

    public class NPCConfiguration : IEntityTypeConfiguration<NPC>
    {
        public void Configure(EntityTypeBuilder<NPC> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.NPCs).HasForeignKey(x => x.UserId);
            builder.ToTable(nameof(NPC), DbSchemes.Characters);
            builder.Property(x => x.Created).HasDefaultValueSql("getutcdate()");
        }
    }
}
