using Microsoft.EntityFrameworkCore;
using server.Model;
namespace server
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SpaceShipProfile> SpaceShipProfiles { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }

            Database.Migrate();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
         .HasMany(u => u.SpaceShipProfiles)
         .WithOne(p => p.User)
         .HasForeignKey(p => p.UserId);
        }

    }
}