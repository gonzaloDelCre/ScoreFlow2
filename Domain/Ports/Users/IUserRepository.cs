using Domain.Entities.Users;

namespace Domain.Ports.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
}
