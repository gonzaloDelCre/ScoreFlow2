using Application.Users.DTOs;
using Domain.Ports.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Access
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public LoginUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDTO> ExecuteAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !VerifyPassword(password, user.PasswordHash.Value))
            {
                return null;
            }

            return new UserResponseDTO
            {
                UserID = user.UserID.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }

        private bool VerifyPassword(string password, string storedPasswordHash)
        {
            return password == storedPasswordHash; 
        }

    }
}
