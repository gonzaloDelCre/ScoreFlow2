using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using System;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Get
{
    public class GetUserByEmailUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByEmailUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get User By Email Case
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserResponseDTO?> ExecuteAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo electrónico no puede estar vacío.", nameof(email));

            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? UserMapper.ToResponseDTO(user) : null;
        }
    }
}
