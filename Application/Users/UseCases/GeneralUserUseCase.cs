using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Ports.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases
{
    public class GeneralUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GeneralUserUseCase> _logger;

        public GeneralUserUseCase(IUserRepository userRepository, ILogger<GeneralUserUseCase> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserDTO> ExecuteAsync(UserDTO userDTO)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userDTO.Email);

            if (existingUser == null)
            {
                _logger.LogInformation("Usuario no encontrado. Creando usuario.");
                return await CreateUser(userDTO);
            }

            if (existingUser.FullName.Value != userDTO.FullName || existingUser.Role != Enum.Parse<UserRole>(userDTO.Role))
            {
                _logger.LogInformation("Usuario encontrado con un nombre o rol diferente. Actualizando usuario.");
                return await UpdateUser(existingUser.UserID.Value, userDTO);
            }

            _logger.LogInformation("El usuario con el mismo email y nombre ya existe.");
            return UserMapper.ToDTO(existingUser);
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<UserDTO> CreateUser(UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Role))
            {
                userDTO.Role = UserRole.Espectador.ToString();
            }

            var user = UserMapper.ToDomain(userDTO);
            var newUser = await _userRepository.AddAsync(user); 

            return UserMapper.ToDTO(newUser);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<UserDTO> UpdateUser(int userId, UserDTO userDTO)
        {
            var existingUser = await _userRepository.GetByIdAsync(new UserID(userId));
            if (existingUser == null) return null; 

            // Validación de datos
            if (string.IsNullOrWhiteSpace(userDTO.Email) || string.IsNullOrWhiteSpace(userDTO.FullName))
            {
                _logger.LogError("Datos de usuario incompletos.");
                throw new ArgumentException("Los datos del usuario son incompletos.");
            }

            // Actualizamos la entidad con los nuevos datos del DTO
            existingUser = new User(
                existingUser.UserID,
                new UserFullName(userDTO.FullName),
                new UserEmail(userDTO.Email),
                new UserPasswordHash(userDTO.PasswordHash),
                Enum.Parse<UserRole>(userDTO.Role),  
                existingUser.CreatedAt 
            );

            await _userRepository.UpdateAsync(existingUser);
            return UserMapper.ToDTO(existingUser);
        }
    }
}
