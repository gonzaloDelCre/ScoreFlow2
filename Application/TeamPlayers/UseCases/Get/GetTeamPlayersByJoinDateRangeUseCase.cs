using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Ports.TeamPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamPlayersByJoinDateRangeUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public GetTeamPlayersByJoinDateRangeUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<List<TeamPlayerResponseDTO>> ExecuteAsync(DateTime from, DateTime to)
        {
            var list = await _repo.GetByJoinDateRangeAsync(from, to);
            return list.Select(tp => tp.ToDTO()).ToList();
        }
    }
}
