using Microsoft.EntityFrameworkCore;
using server.Model;
public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        System.Console.WriteLine("trying to init");
        if (!Database.CanConnect())
        {
            System.Console.WriteLine("cannect fail");
            Database.EnsureCreated();
        }
        // To be test...
        //Database.Migrate();
        
    }

    public DbSet<User> Users { get; set; }
}
