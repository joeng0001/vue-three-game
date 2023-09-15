using Microsoft.EntityFrameworkCore;
using connectMySQL.Model;
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
