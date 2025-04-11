using Application.TeamPlayers.DTOs;
using Domain.Services;
using Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Services.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamPlayersByPlayerId
    {
        private readonly TeamPlayerService _teamPlayerService;

        public GetTeamPlayersByPlayerId(TeamPlayerService teamPlayerService)
        {
            _teamPlayerService = teamPlayerService;
        }

        public async Task<IEnumerable<TeamPlayerResponseDTO>> ExecuteAsync(int playerId)
        {
            var result = await _teamPlayerService.GetByPlayerIdAsync(new PlayerID(playerId));
            return result.Select(tp => tp.ToResponseDTO());
        }
    }
}
