using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using Domain.Shared;

namespace Application.Users.UseCases
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> ExecuteAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(new UserID(userId));
            return user != null ? UserMapper.ToDTO(user) : null;
        }
    }
}
