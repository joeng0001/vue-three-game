using Microsoft.EntityFrameworkCore;
using server.Model;
public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        if (!Database.CanConnect())
        {
            Database.EnsureCreated();
        }
        // To be test...
        //Database.Migrate();
        
    }

    public DbSet<User> Users { get; set; }
}
