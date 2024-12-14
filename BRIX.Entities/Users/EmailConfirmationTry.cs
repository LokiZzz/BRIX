using BRIX.GameService.Entities.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BRIX.GameService.Entities.Users
{
    public class EmailConfirmationTries
    {
        public Guid Id { get; set; }

        public int Count { get; set; }

        public DateTime LastTryDateTimeUtc { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;
    }

    public class EmailConfirmationTryConfiguration : IEntityTypeConfiguration<EmailConfirmationTries>
    {
        public void Configure(EntityTypeBuilder<EmailConfirmationTries> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.EmailConfirmationTries).HasForeignKey(x => x.UserId);
            builder.ToTable(nameof(EmailConfirmationTries), DbSchemes.Accounts);
        }
    }
}
