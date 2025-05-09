using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Domain.Ports.Standings;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetStandingByTeamAndLeagueUseCase
    {
        private readonly IStandingRepository _repo;
        public GetStandingByTeamAndLeagueUseCase(IStandingRepository repo) => _repo = repo;

        public async Task<StandingResponseDTO?> ExecuteAsync(int teamId, int leagueId)
        {
            var s = await _repo.GetByTeamIdAndLeagueIdAsync(
                new TeamID(teamId), new LeagueID(leagueId)
            );
            return s is null ? null : s.ToDTO();
        }
    }
}
