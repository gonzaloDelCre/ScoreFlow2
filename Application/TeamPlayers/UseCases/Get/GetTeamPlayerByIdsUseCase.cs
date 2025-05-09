using Application.TeamPlayers.DTOs;
using Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Ports.TeamPlayers;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamPlayerByIdsUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public GetTeamPlayerByIdsUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<TeamPlayerResponseDTO?> ExecuteAsync(int teamId, int playerId)
        {
            var tp = await _repo.GetByIdsAsync(
                new TeamID(teamId),
                new PlayerID(playerId)
            );
            return tp is null ? null : tp.ToDTO();
        }
    }
}
