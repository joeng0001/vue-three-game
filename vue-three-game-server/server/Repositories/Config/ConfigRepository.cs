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



        public async Task<Boolean> AddSpaceShipProfile(User user, SpaceShipProfileReq s)
        {
            var profile = new SpaceShipProfile
            {
                ammo = s.ammo,
                life = s.life,
                energy = s.energy,
                energyConsume = s.energyConsume,
                lifeConsume = s.lifeConsume,
                UserId = user.Id
            };

            if (user.SpaceShipProfiles == null)
            {
                user.SpaceShipProfiles = new List<SpaceShipProfile>();
            }
            if (user.SpaceShipProfiles.Count == 5)
            {
                return false;
            }

            user.SpaceShipProfiles.Add(profile);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Boolean> AddMarsRoverProfile(User user, MarsRoverProfileReq s)
        {
            var profile = new MarsRoverProfile
            {
                oil = s.oil,
                energy = s.energy,
                UserId = user.Id
            };

            if (user.MarsRoverProfiles == null)
            {
                user.MarsRoverProfiles = new List<MarsRoverProfile>();
            }
            if (user.MarsRoverProfiles.Count == 5)
            {
                return false;
            }

            user.MarsRoverProfiles.Add(profile);

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<Boolean> UpdateSpaceShipProfile(User user, int profileID, SpaceShipProfileReq s)
        {
            var profile = user.SpaceShipProfiles.FirstOrDefault(prof => prof.Id == profileID);
            
            if (profile != null)
            {
                System.Console.WriteLine("find profile!");
                profile.life = s.life;
                profile.ammo = s.ammo;
                profile.energy = s.energy;
                profile.energyConsume=s.energyConsume;
                profile.lifeConsume=s.lifeConsume;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Boolean> UpdateMarsRoverProfile(User user, int profileID, MarsRoverProfileReq s)
        {

            var profile = user.MarsRoverProfiles.FirstOrDefault(prof => prof.Id == profileID);
            if (profile != null)
            {
                profile.energy = s.energy;
                profile.oil = s.oil;
                await _context.SaveChangesAsync();
            }
            return true;
        }


        public ICollection<SpaceShipProfileRes> GetSpaceShipProfiles(User user)
        {

            if (user.SpaceShipProfiles == null)
            {
                user.SpaceShipProfiles = new List<SpaceShipProfile>();
            }

            ICollection <SpaceShipProfileRes> list = new List<SpaceShipProfileRes>();

            foreach (SpaceShipProfile obj in user.SpaceShipProfiles)
            {

                list.Add(new SpaceShipProfileRes(obj));
            }
            return list;
        }

        public ICollection<MarsRoverProfileRes> GetMarsRoverProfiles(User user)
        {

            if (user.MarsRoverProfiles == null)
            {
                user.MarsRoverProfiles = new List<MarsRoverProfile>();
            }

            ICollection<MarsRoverProfileRes> list = new List<MarsRoverProfileRes>();

            foreach (MarsRoverProfile obj in user.MarsRoverProfiles)
            {

                list.Add(new MarsRoverProfileRes(obj));
            }
            return list;
        }
    }
}
