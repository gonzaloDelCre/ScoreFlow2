using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Ports.Users;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Update
{
    public class ChangeUserPasswordUseCase
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserPasswordUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ExecuteAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(new UserID(userId));
            if (user == null)
                return false;

            
            if (!VerifyPassword(user, currentPassword))
                return false;

            // Establecer la nueva contraseña
            user.SetPassword(new (HashPassword(newPassword)));

            await _userRepository.UpdateAsync(user);
            return true;
        }

        // Estos métodos dependerán de tu implementación específica
        private bool VerifyPassword(Domain.Entities.Users.User user, string password)
        {
            return true; 
        }

        private string HashPassword(string password)
        {
            return password; 
        }
    }
}
