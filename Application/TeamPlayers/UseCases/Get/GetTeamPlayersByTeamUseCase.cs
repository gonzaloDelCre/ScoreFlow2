using Application.TeamPlayers.DTOs;
using Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Ports.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamPlayersByTeamUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public GetTeamPlayersByTeamUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<List<TeamPlayerResponseDTO>> ExecuteAsync(int teamId)
        {
            var list = await _repo.GetByTeamIdAsync(new TeamID(teamId));
            return list.Select(tp => tp.ToDTO()).ToList();
        }
    }
}
 