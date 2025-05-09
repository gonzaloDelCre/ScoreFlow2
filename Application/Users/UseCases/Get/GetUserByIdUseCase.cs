using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Get
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _repo;
        public GetUserByIdUseCase(IUserRepository repo) => _repo = repo;

        public async Task<UserResponseDTO?> ExecuteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(new UserID(id));
            return user is null ? null : user.ToDTO();
        }
    }
}
