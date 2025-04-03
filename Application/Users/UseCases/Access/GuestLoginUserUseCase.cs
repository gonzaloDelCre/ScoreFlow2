using Application.Users.DTOs;
using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Access
{
    public class GuestLoginUseCase
    {
        private readonly UserService _userService;

        public GuestLoginUseCase(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserResponseDTO> ExecuteAsync()
        {
            var guestUser = await _userService.CreateGuestUserAsync();

            return new UserResponseDTO
            {
                UserID = guestUser.UserID.Value,
                FullName = guestUser.FullName.Value,
                Email = guestUser.Email.Value,
                Role = guestUser.Role.ToString()
            };
        }
    }
}
