using Microsoft.EntityFrameworkCore;
using server.Model;
public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public  UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        if (!Database.CanConnect())
        {
            Database.EnsureCreated();
            if (!Users.Any())
            {
                Users.Add(new User { Name = "John Doe", password=new byte[1024],salt=new byte[10] });
                SaveChanges(); 
            }
        }

        Database.Migrate();
        
    }

    
}
