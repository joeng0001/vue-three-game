using System.Diagnostics.Eventing.Reader;
using server.Model;
using Microsoft.EntityFrameworkCore;

namespace server.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Delete(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
  
        }
        public async Task<IEnumerable<User>> GetList()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(e=>e.Name==name);
        }

    }
}
