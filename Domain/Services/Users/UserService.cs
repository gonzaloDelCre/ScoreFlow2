using Domain.Entities.Users;
using Domain.Enum;
using Domain.Ports.Users;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
namespace Domain.Services.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<User> CreateUserAsync(string fullName, string email, string passwordHash, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre completo es obligatorio.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo electrónico es obligatorio.");

            var user = new User(
                new UserID(0), // Será autoincremental en la BD
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

        public async Task<User> RegisterUserAsync(string fullName, string email, string password, UserRole role = UserRole.Espectador)
        {
            // Verificar si el usuario ya existe
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Ya existe un usuario con este correo electrónico.");
            }

            // Generar hash de la contraseña
            string passwordHash = HashPassword(password);

            // Crear y guardar el nuevo usuario
            var user = await CreateUserAsync(fullName, email, passwordHash, role);

            return user;
        }

        public async Task<(User user, string token)> LoginAsync(string email, string password)
        {
            // Buscar el usuario por email
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Correo electrónico o contraseña incorrectos.");
            }

            // Verificar la contraseña
            if (!VerifyPassword(password, user.PasswordHash.Value))
            {
                throw new UnauthorizedAccessException("Correo electrónico o contraseña incorrectos.");
            }

            // Generar token JWT
            string token = GenerateJwtToken(user);

            return (user, token);
        }

        // Métodos auxiliares para el manejo de contraseñas y tokens
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash == storedHash;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
                new Claim("role", user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
