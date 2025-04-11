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
    public class GetTeamPlayersByTeamId
    {
        private readonly TeamPlayerService _teamPlayerService;

        public GetTeamPlayersByTeamId(TeamPlayerService teamPlayerService)
        {
            _teamPlayerService = teamPlayerService;
        }

        public async Task<IEnumerable<TeamPlayerResponseDTO>> ExecuteAsync(int teamId)
        {
            var result = await _teamPlayerService.GetByTeamIdAsync(new TeamID(teamId));
            return result.Select(tp => tp.ToResponseDTO());
        }
    }
}
