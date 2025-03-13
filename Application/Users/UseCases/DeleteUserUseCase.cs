using Domain.Ports.Users;
using Microsoft.Extensions.Logging;
using Domain.Shared;

namespace Application.Users.UseCases
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserUseCase> _logger;

        public DeleteUserUseCase(IUserRepository userRepository, ILogger<DeleteUserUseCase> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> ExecuteAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(new UserID(userId));
            if (user == null)
            {
                _logger.LogError("Usuario con ID {UserID} no encontrado.", userId);
                return false;
            }

            var result = await _userRepository.DeleteAsync(new UserID(userId));
            return result;
        }
    }
}
