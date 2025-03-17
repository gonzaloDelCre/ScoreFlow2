using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Get
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDTO?> ExecuteAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(new UserID(userId));
            return user != null ? UserMapper.ToResponseDTO(user) : null;
        }
    }
}
