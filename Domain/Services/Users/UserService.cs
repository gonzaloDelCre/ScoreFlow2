using Domain.Entities.Users;
using Domain.Enum;
using Domain.Ports.Users;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
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

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update User 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");

            await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get User By Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<User> RegisterAsync(string fullName, string email, string password, UserRole role)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("El correo electrónico ya está registrado.");
            }

            var user = new User(
                new UserID(0),  
                new UserFullName(fullName),
                new UserEmail(email),
                new UserPasswordHash(password), 
                role,
                DateTime.UtcNow
            );

            await _userRepository.AddAsync(user);
            return user;
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.PasswordHash.Value != password)
            {
                throw new UnauthorizedAccessException("Correo electrónico o contraseña incorrectos.");
            }
            return user;
        }

        /// <summary>
        /// Create Guest User
        /// </summary>
        /// <returns></returns>
        public Task<User> CreateGuestUserAsync()
        {
            var guestUser = new User(
                new UserID(0),
                new UserFullName("Usuario Invitado"),
                new UserEmail("guest@scoreflow.com"), 
                new UserPasswordHash(""),
                UserRole.Espectador,
                DateTime.UtcNow
            );

            return Task.FromResult(guestUser); 
        }

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetProfileAsync(UserID userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado.");

            return user;
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fullName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<User> UpdateProfileAsync(UserID userId, string fullName, string email, string? password, string role)
        {
            _logger.LogInformation("Buscando usuario con ID {UserID} para actualizar perfil", userId.Value);
            var user = await _userRepository.GetByIdAsync(userId);
            _logger.LogInformation("Actualizando perfil para el usuario con ID {UserID}", userId);

            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado.");

            user.UpdateFullName(new UserFullName(fullName));
            user.UpdateEmail(new UserEmail(email));
            user.UpdateRole(role);
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.UpdatePassword(new UserPasswordHash(password));
            }

            await _userRepository.UpdateAsync(user);
            return user;
        }

    }
}
