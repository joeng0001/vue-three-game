using server.Model;

namespace server.Repositories
{
    public interface IConfigRepository
    {

        SpaceShipProfileRes GetSpaceShipProfileByID(User user,int id);

        MarsRoverProfileRes GetMarsRoverProfileByID(User user, int id);

        Task<Boolean> AddSpaceShipProfile(User user, SpaceShipProfileReq s);

        Task<Boolean> AddMarsRoverProfile(User user, MarsRoverProfileReq s);

        Task<Boolean> UpdateSpaceShipProfile(User user, int profileID, SpaceShipProfileReq s);

        Task<Boolean> UpdateMarsRoverProfile(User user, int profileID, MarsRoverProfileReq s);

        ICollection<SpaceShipProfileRes> GetSpaceShipProfiles(User user);

        ICollection<MarsRoverProfileRes> GetMarsRoverProfiles(User user);
    }
}
