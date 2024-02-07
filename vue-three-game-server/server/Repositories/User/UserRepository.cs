using System.Diagnostics.Eventing.Reader;
using server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
            return await _context.Users.Include(u => u.SpaceShipProfiles).ToListAsync();
        }
        public async Task<User> Get(int id)
        {
            User user=await _context.Users.FindAsync(id);
            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(u => u.SpaceShipProfiles)
                    .LoadAsync();
            }

            return user;
        }
        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByName(string name)
        {
            User user=await _context.Users.FirstOrDefaultAsync(e=>e.Name==name);
            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(u => u.SpaceShipProfiles)
                    .LoadAsync();
            }
            return user;
        }

        public async Task<Boolean> AddSpaceShipProfile(User user, SpaceShipProfileReq s)
        {
            var profile = new SpaceShipProfile {
                ammo = s.ammo,
                life = s.life,
                energy = s.energy,
                energyConsume = s.energyConsume,
                lifeConsume = s.lifeConsume,
                UserId=user.Id
            };
            
            if (user.SpaceShipProfiles == null)
            {
                user.SpaceShipProfiles = new List<SpaceShipProfile>();
            }
            if(user.SpaceShipProfiles.Count == 5)
            {
                return false;
            }

            user.SpaceShipProfiles.Add(profile);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
