using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Create
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _repo;
        public LoginUserUseCase(IUserRepository repo) => _repo = repo;

        public async Task<UserResponseDTO?> ExecuteAsync(LoginRequestDTO dto)
        {
            var user = await _repo.AuthenticateAsync(dto.Email, dto.PasswordHash);
            return user?.ToDTO();
        }
    }
}
