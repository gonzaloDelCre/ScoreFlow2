using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class GetTeamPlayersUseCase
    {
        private readonly ITeamRepository _repo;
        public GetTeamPlayersUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<List<PlayerResponseDTO>> ExecuteAsync(int teamId)
        {
            var players = await _repo.GetPlayersAsync(new TeamID(teamId));
            return players.Select(p => p.ToDTO()).ToList();
        }
    }
}
