using System.Diagnostics.Metrics;
using server.Model;
namespace server
{
    public class Seed
    {

        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Users.Any())
            {
                dataContext.Add(new User { Name = "John Doe", password = new byte[10], salt = new byte[10] });
                dataContext.SaveChanges();
            }
        }
        
    }
}
