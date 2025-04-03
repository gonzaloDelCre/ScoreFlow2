using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Entities.Users;
using Domain.Ports.Users;
using Domain.Services.Users;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Update
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;

        public UpdateUserUseCase(IUserRepository userRepository, UserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        /// <summary>
        /// Update User Case
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UserResponseDTO> ExecuteAsync(UserRequestDTO userDTO)
        {
            if (userDTO == null || !userDTO.UserID.HasValue)
                throw new ArgumentNullException(nameof(userDTO), "Los detalles del usuario no pueden ser nulos.");

            var existingUser = await _userRepository.GetByIdAsync(new UserID(userDTO.UserID.Value));
            if (existingUser == null)
                throw new InvalidOperationException("El usuario no existe. No se puede actualizar.");

            existingUser.Update(
                new UserFullName(userDTO.FullName),
                new UserEmail(userDTO.Email),
                new UserPasswordHash(userDTO.PasswordHash)
            );

            await _userRepository.UpdateAsync(existingUser);
            return UserMapper.ToResponseDTO(existingUser);
        }


    }
}
