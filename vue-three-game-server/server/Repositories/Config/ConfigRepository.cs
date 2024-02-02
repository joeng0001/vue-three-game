using System.Diagnostics.Eventing.Reader;
using server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace server.Repositories
{
    public class ConfigRepository: IConfigRepository
    {
        private readonly DataContext _context;

        public ConfigRepository(DataContext context)
        {
            _context = context;
        }

       
        public SpaceShipProfileRes GetSpaceShipProfileByID(User user, int id)
        {

            if (user.SpaceShipProfiles == null)
            {
                user.SpaceShipProfiles = new List<SpaceShipProfile>();
            }
            SpaceShipProfile s = user.SpaceShipProfiles.FirstOrDefault(p => p.Id == id);

            if (s != null)
            {
                return new SpaceShipProfileRes(s);
            }
            else
            {
                return null;
            }
        }
    }
}
