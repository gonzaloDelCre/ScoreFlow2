using Application.Users.DTOs;
using Domain.Ports.Users;
using Domain.Services.Users;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Profile
{
    public class GetUserProfileUseCase
    {
        private readonly UserService _userService;

        public GetUserProfileUseCase(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserProfileResponseDTO> ExecuteAsync(UserID userId)
        {
            var user = await _userService.GetProfileAsync(userId);
            return new UserProfileResponseDTO
            {
                UserID = user.UserID.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
