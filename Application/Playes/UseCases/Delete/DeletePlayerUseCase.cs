using Domain.Ports.Players;
using Domain.Shared;

namespace Application.Playes.UseCases.Delete
{
    public class DeletePlayerUseCase
    {
        private readonly IPlayerRepository _repo;
        public DeletePlayerUseCase(IPlayerRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id)
            => _repo.DeleteAsync(new PlayerID(id));
    }
}
