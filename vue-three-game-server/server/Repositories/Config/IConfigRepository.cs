using server.Model;

namespace server.Repositories
{
    public interface IConfigRepository
    {

        SpaceShipProfileRes GetSpaceShipProfileByID(User user,int id);
    }
}
