using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Get
{
    public class GetAllUsersUseCase
    {
        private readonly IUserRepository _repo;
        public GetAllUsersUseCase(IUserRepository repo) => _repo = repo;

        public async Task<List<UserResponseDTO>> ExecuteAsync()
        {
            var users = await _repo.GetAllAsync();
            return users.Select(u => u.ToDTO()).ToList();
        }
    }
}
