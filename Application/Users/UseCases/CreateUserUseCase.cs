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
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CreateUserUseCase> _logger;

        public CreateUserUseCase(IUserRepository userRepository, ILogger<CreateUserUseCase> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserDTO> ExecuteAsync(UserDTO userDTO)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userDTO.Email);

            if (existingUser != null)
            {
                _logger.LogWarning("El usuario con correo {Email} ya existe. No se puede crear uno nuevo.", userDTO.Email);
                return null;
            }

            var user = UserMapper.ToDomain(userDTO);
            var createdUser = await _userRepository.AddAsync(user);
            _logger.LogInformation("Nuevo usuario creado con correo {Email}.", userDTO.Email);

            return UserMapper.ToDTO(createdUser);
        }
    }
}
