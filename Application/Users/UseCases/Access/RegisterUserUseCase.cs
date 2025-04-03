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

        /// <summary>
        /// Register User Case
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UserResponseDTO> ExecuteAsync(RegisterRequestDTO registerRequest)
        {
            try
            {
                var role = Enum.Parse<UserRole>(registerRequest.Role);

                var user = await _userService.RegisterAsync(registerRequest.FullName, registerRequest.Email, registerRequest.Password, role);

                return new UserResponseDTO
                {
                    FullName = user.FullName.Value,
                    Email = user.Email.Value,
                    Role = user.Role.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al registrar el usuario: {ex.Message}", ex);
            }
        }

    }
}
