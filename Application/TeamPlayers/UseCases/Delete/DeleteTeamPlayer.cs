using Domain.Services;
using Domain.Services.TeamPlayers;
using Domain.Shared;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Delete
{
    public class DeleteTeamPlayer
    {
        private readonly TeamPlayerService _teamPlayerService;

        public DeleteTeamPlayer(TeamPlayerService teamPlayerService)
        {
            _teamPlayerService = teamPlayerService;
        }

        public async Task<bool> ExecuteAsync(int teamId, int playerId)
        {
            return await _teamPlayerService.DeleteAsync(new TeamID(teamId), new PlayerID(playerId));
        }
    }
}
