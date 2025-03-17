using Domain.Entities.Users;
using Domain.Enum;
using Domain.Ports.Users;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> CreateUserAsync(string fullName, string email, string passwordHash, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre completo es obligatorio.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo electrónico es obligatorio.");

            var user = new User(
                new UserID(0),  
                new UserFullName(fullName),
                new UserEmail(email),
                new UserPasswordHash(passwordHash),
                role,
                DateTime.UtcNow
            );

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(UserID userId)
        {
            try
            {
                return await _userRepository.GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {UserID}.", userId.Value);
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");

            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(UserID userId)
        {
            try
            {
                return await _userRepository.DeleteAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID {UserID}.", userId.Value);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios.");
                throw;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userRepository.GetByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con correo electrónico {Email}.", email);
                throw;
            }
        }
    }
}
