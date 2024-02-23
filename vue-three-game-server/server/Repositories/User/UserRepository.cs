using System.Diagnostics.Eventing.Reader;
using server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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
            return await _context.Users
                .Include(u => u.SpaceShipProfiles)
                .Include(u => u.MarsRoverProfiles)
                .ToListAsync();
        }
        public async Task<User> Get(int id)
        {
            User user=await _context.Users.FindAsync(id);
            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(u => u.SpaceShipProfiles)
                    .LoadAsync();
                await _context.Entry(user)
                    .Collection(u => u.MarsRoverProfiles)
                    .LoadAsync();
            }

            return user;
        }

        public async Task<User> GetUserByName(string name)
        {
            User user=await _context.Users.FirstOrDefaultAsync(e=>e.Name==name);
            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(u => u.SpaceShipProfiles)
                    .LoadAsync();

                await _context.Entry(user)
                    .Collection(u => u.MarsRoverProfiles)
                    .LoadAsync();
            }
            return user;
        }

        
    }
}
