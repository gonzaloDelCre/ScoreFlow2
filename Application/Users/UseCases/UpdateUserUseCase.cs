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
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserUseCase> _logger;

        public UpdateUserUseCase(IUserRepository userRepository, ILogger<UpdateUserUseCase> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserDTO> ExecuteAsync(UserDTO userDTO)
        {
            var existingUser = await _userRepository.GetByIdAsync(new UserID(userDTO.UserID));
            if (existingUser == null)
            {
                _logger.LogWarning("El usuario con ID {UserID} no existe. No se puede actualizar.", userDTO.UserID);
                return null;
            }

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
