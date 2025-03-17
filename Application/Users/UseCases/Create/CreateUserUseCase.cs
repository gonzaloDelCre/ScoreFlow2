using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Ports.Users;
using Domain.Services.Users;
using Domain.Enum;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Create
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;

        public CreateUserUseCase(IUserRepository userRepository, UserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        public async Task<UserResponseDTO> ExecuteAsync(UserRequestDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO), "Los detalles del usuario no pueden ser nulos.");

            var existingUser = await _userRepository.GetByEmailAsync(userDTO.Email);
            if (existingUser != null)
                throw new ArgumentException("Ya existe un usuario con ese correo electrónico.");

            var user = await _userService.CreateUserAsync(
                userDTO.FullName, userDTO.Email, userDTO.PasswordHash, Enum.TryParse<UserRole>(userDTO.Role, out var role) ? role : UserRole.Espectador
            );

            return new UserResponseDTO
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
