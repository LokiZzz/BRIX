using BRIX.Web.Data.Entities.Characters;
using BRIX.Web.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BRIX.Web.Data
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
        /// Персонажи пользователя.
        /// </summary>
        public DbSet<UserCharacter> UserCharacters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserCharacterConfiguration).Assembly);

            // Переопределение названий таблиц Identity
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
