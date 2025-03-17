using Application.Matches.DTOs;
using Domain.Ports.Matches;
using Domain.Services.Matches;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Get
{
    public class GetAllMatches
    {
        private readonly MatchService _matchService;

        public GetAllMatches(MatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task<IEnumerable<MatchResponseDTO>> ExecuteAsync()
        {
            var matches = await _matchService.GetAllMatchesAsync();
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
