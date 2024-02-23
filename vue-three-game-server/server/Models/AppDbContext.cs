using Microsoft.EntityFrameworkCore;
using server.Model;
using System.Reflection.PortableExecutable;
namespace server
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SpaceShipProfile> SpaceShipProfiles { get; set; }

        public DbSet<MarsRoverProfile> MarsRoverProfiles { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

            Boolean connected = (Database.GetDbConnection().State == System.Data.ConnectionState.Open);
            while (!connected)
            {
                try
                {
                    Database.EnsureCreated();
                    Database.OpenConnection();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("waiting for db connection...");
                }
                Thread.Sleep(5000);
                connected = (Database.GetDbConnection().State == System.Data.ConnectionState.Open);
            }
            System.Console.WriteLine("connected");
            Database.EnsureCreated();
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>() // link User to SpaceShipProfiles
            .HasMany(u => u.SpaceShipProfiles)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>() //link User to MarsRoverProfiles
           .HasMany(u => u.MarsRoverProfiles)
           .WithOne(p => p.User)
           .HasForeignKey(p => p.UserId);
        }

    }
}