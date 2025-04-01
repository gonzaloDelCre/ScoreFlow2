using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Ports.Users;
using Domain.Services.Users;
using Domain.Shared;
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
        private readonly UserService _userService;

        public RegisterUserUseCase(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserResponseDTO> ExecuteAsync(RegisterRequestDTO registerRequest)
        {
            // Delegar la creación de usuario al servicio
            var role = Enum.Parse<UserRole>(registerRequest.Role);

            var user = await _userService.RegisterAsync(registerRequest.FullName, registerRequest.Email, registerRequest.Password, role);

            // Mapear el usuario a un DTO de respuesta
            return new UserResponseDTO
            {
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                Role = user.Role.ToString()
            };
        }
    }
}
