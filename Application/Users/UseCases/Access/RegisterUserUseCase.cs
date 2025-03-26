using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Ports.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Access
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterUserUseCase> _logger;

        public RegisterUserUseCase(IUserRepository userRepository, ILogger<RegisterUserUseCase> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserResponseDTO> ExecuteAsync(RegisterRequestDTO registerDTO)
        {
            // Validar que el correo no exista en la base de datos
            var existingUser = await _userRepository.GetByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("El correo electrónico ya está registrado.");
            }

            var role = Enum.Parse<UserRole>(registerDTO.Role);  // Convierte el string a UserRole

            var user = new User(registerDTO.FullName, registerDTO.Email, registerDTO.Password, role);


            // Guardar el nuevo usuario en la base de datos
            await _userRepository.AddAsync(user);

            // Mapear el usuario creado a un UserResponseDTO
            var userResponse = UserMapper.ToResponseDTO(user);

            // Log de éxito
            _logger.LogInformation($"Nuevo usuario registrado: {registerDTO.Email}");

            return userResponse;
        }
    }
}
