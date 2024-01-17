using Microsoft.EntityFrameworkCore;
using AuthService.Interfaces;
using AuthService.Interfaces;

namespace AuthService.Context
{
    public class AuthDatabaseContext : DbContext
    {
        public AuthDatabaseContext(DbContextOptions<AuthDatabaseContext> options) : base(options)
        {

        }
        public DbSet<ILoginUser> LoginUsers { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<IUser>()
                .Property(e => e.id)
                .HasConversion(
                    v => v.ToString(),
                    v => Guid.Parse(v));
        }*/
    }

}
