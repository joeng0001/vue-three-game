using Microsoft.EntityFrameworkCore;
using server.Model;
namespace server
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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
            //modelBuilder.Entity<ChildEntity>()
            //    .HasOne(c => c.ParentEntity)
            //    .WithMany(p => p.Children)
            //    .HasForeignKey(c => c.ParentEntityId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //base.OnModelCreating(modelBuilder);
        }

    }
}