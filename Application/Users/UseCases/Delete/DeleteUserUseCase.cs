using Domain.Ports.Users;
using Domain.Services.Users;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Delete
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserUseCase> _logger;
        private readonly UserService _userService;

        public DeleteUserUseCase(IUserRepository userRepository, ILogger<DeleteUserUseCase> logger, UserService userService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Delete User Case
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> ExecuteAsync(UserID userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId), "El ID del usuario no puede ser nulo.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Usuario con ID {UserID} no encontrado.", userId.Value);
                return false;
            }

            return await _userService.DeleteUserAsync(userId);
        }
    }
}
