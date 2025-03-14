using Application.Matches.DTOs;
using Domain.Services.Matches;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Get
{
    public class GetMatchesByTeamId
    {
        private readonly MatchService _matchService;

        public GetMatchesByTeamId(MatchService matchService)
        {
            _matchService = matchService;
        }

        // Ejecuta la obtención de partidos para un equipo específico
        public async Task<IEnumerable<MatchResponseDTO>> Execute(MatchID teamId)
        {
            var matches = await _matchService.GetMatchesByTeamIdAsync(teamId);
            return matches.Select(match => new MatchResponseDTO
            {
                MatchID = match.MatchID,
                Team1 = match.Team1,
                Team2 = match.Team2,
                MatchDate = match.MatchDate,
                Status = match.Status.ToString(),
                Location = match.Location,
                CreatedAt = match.CreatedAt
            });
        }
    }
}
