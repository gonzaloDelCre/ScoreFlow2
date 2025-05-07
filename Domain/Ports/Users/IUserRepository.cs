using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Ports.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserID userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(UserID userId);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> AuthenticateAsync(string email, string passwordHash);

    }
}
