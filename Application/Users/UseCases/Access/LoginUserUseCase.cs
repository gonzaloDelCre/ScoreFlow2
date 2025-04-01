using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Access
{
    public class LoginUserUseCase
    {
        private readonly UserService _userService;

        public LoginUserUseCase(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserResponseDTO> ExecuteAsync(string email, string password)
        {
            var user = await _userService.LoginAsync(email, password);
            return UserMapper.ToResponseDTO(user);
        }
    }
}
