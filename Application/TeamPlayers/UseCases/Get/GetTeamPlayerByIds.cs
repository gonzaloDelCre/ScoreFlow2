using Application.TeamPlayers.DTOs;
using Domain.Services;
using Domain.Shared;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Services.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamPlayerByIds
    {
        private readonly TeamPlayerService _teamPlayerService;

        public GetTeamPlayerByIds(TeamPlayerService teamPlayerService)
        {
            _teamPlayerService = teamPlayerService;
        }

        public async Task<TeamPlayerResponseDTO?> ExecuteAsync(int teamId, int playerId)
        {
            var result = await _teamPlayerService.GetByIdsAsync(new TeamID(teamId), new PlayerID(playerId));
            return result?.ToResponseDTO();
        }
    }
}
