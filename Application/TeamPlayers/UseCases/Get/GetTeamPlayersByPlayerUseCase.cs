using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Ports.TeamPlayers;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamPlayersByPlayerUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public GetTeamPlayersByPlayerUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<List<TeamPlayerResponseDTO>> ExecuteAsync(int playerId)
        {
            var list = await _repo.GetByPlayerIdAsync(new PlayerID(playerId));
            return list.Select(tp => tp.ToDTO()).ToList();
        }
    }
}
