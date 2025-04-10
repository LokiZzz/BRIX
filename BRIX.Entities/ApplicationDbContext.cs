using BRIX.GameService.Entities.Characters;
using BRIX.GameService.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BRIX.GameService.Entities
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<User, Role, Guid,
            IdentityUserClaim<Guid>,
            IdentityUserRole<Guid>,
            IdentityUserLogin<Guid>,
            IdentityRoleClaim<Guid>,
            IdentityUserToken<Guid>>(options)
    {
        /// <summary>
        /// ��������� ������.
        /// </summary>
        public DbSet<PlayerCharacter> PlayerCharacters { get; set; } = default!;

        /// <summary>
        /// NPC
        /// </summary>
        public DbSet<NPC> NPCs { get; set; } = default!;

        /// <summary>
        /// ������� ������������� �������� ����� �����.
        /// </summary>
        public DbSet<EmailConfirmationTries> EmailConfirmationTries { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlayerCharacterConfiguration).Assembly);

            // ��������������� �������� ������ Identity
            modelBuilder.Entity<User>().ToTable(nameof(User), DbSchemes.Accounts);
            modelBuilder.Entity<Role>().ToTable(nameof(Role), DbSchemes.Accounts);
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole", DbSchemes.Accounts);
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim", DbSchemes.Accounts);
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin", DbSchemes.Accounts);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim", DbSchemes.Accounts);
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken", DbSchemes.Accounts);
        }
    }
}
