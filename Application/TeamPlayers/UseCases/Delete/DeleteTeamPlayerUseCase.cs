using Domain.Ports.TeamPlayers;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Delete
{
    public class DeleteTeamPlayerUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public DeleteTeamPlayerUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int teamId, int playerId)
            => _repo.DeleteAsync(new TeamID(teamId), new PlayerID(playerId));
    }
}
