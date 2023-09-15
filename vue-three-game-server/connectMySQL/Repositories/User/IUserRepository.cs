using connectMySQL.Model;

namespace connectMySQL.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetList();

        Task<User> Get(int id);

        Task<User> Create(User user);

        Task Update(User user);

        Task Delete(int id);
    }
}
