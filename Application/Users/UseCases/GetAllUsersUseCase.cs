using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases
{
    public class GetAllUsersUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> ExecuteAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(UserMapper.ToDTO);
        }
    }
}
