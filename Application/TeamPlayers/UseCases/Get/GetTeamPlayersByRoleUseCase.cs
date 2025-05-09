using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Enum;
using Domain.Ports.TeamPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.UseCases.Get
{
    public class GetTeamPlayersByRoleUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public GetTeamPlayersByRoleUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<List<TeamPlayerResponseDTO>> ExecuteAsync(RoleInTeam role)
        {
            var list = await _repo.GetByRoleAsync(role);
            return list.Select(tp => tp.ToDTO()).ToList();
        }
    }
}
