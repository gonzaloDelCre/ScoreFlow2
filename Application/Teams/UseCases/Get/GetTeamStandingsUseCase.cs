using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class GetTeamStandingsUseCase
    {
        private readonly ITeamRepository _repo;
        public GetTeamStandingsUseCase(ITeamRepository repo) => _repo = repo;

        public async Task<List<StandingResponseDTO>> ExecuteAsync(int teamId)
        {
            var stand = await _repo.GetStandingsAsync(new TeamID(teamId));
            return stand.Select(s => s.ToDTO()).ToList();
        }
    }
}
