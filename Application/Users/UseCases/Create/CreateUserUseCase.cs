using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Ports.Users;
using Application.Users.Mapper;

namespace Application.Users.UseCases.Create
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _repo;
        public CreateUserUseCase(IUserRepository repo) => _repo = repo;

        public async Task<UserResponseDTO> ExecuteAsync(UserRequestDTO dto)
        {
            var user = dto.ToDomain();
            var created = await _repo.AddAsync(user);
            return created.ToDTO();
        }
    }
}
