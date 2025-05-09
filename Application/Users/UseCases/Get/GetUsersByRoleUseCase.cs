using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Enum;
using Domain.Ports.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Get
{
    public class GetUsersByRoleUseCase
    {
        private readonly IUserRepository _repo;
        public GetUsersByRoleUseCase(IUserRepository repo) => _repo = repo;

        public async Task<List<UserResponseDTO>> ExecuteAsync(UserRole role)
        {
            var users = await _repo.GetByRoleAsync(role);
            return users.Select(u => u.ToDTO()).ToList();
        }
    }
}
