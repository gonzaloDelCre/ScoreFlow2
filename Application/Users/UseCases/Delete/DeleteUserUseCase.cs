using Domain.Ports.Users;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Delete
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _repo;
        public DeleteUserUseCase(IUserRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id) =>
            _repo.DeleteAsync(new UserID(id));
    }
}
