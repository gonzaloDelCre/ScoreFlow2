using Domain.Entities.Users;
using Domain.Enum;
using Domain.Ports.Users;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            // Usar los constructores de las entidades correspondientes
            var user = new User(
                new UserID(0), // Será autoincremental en la BD
                new UserFullName(fullName),
                new UserEmail(email),
                new UserPasswordHash(passwordHash), // Usar el tipo correspondiente para la contraseña
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

        public async Task<User> RegisterAsync(string fullName, string email, string password)
        {
            // Verificar si el email ya está registrado
            var existingUser = await GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("El correo electrónico ya está registrado.");
            }

            // Crear el nuevo usuario
            var user = new User(
                new UserID(0), // Será autoincremental en la BD
                new UserFullName(fullName),
                new UserEmail(email),
                new UserPasswordHash(password), // Usar el tipo correspondiente para la contraseña
                UserRole.Jugador,  // O el rol que corresponda
                DateTime.UtcNow
            );

            return await CreateUserAsync(fullName, email, password, UserRole.Jugador);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            // Buscar el usuario por email
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.PasswordHash.Value != password)  // Comparar con el valor del hash
            {
                throw new UnauthorizedAccessException("Correo electrónico o contraseña incorrectos.");
            }

            return user;
        }
    }
}
