using Application.Matches.DTOs;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Services.Matches;
using Domain.Shared;

namespace Application.Matches.UseCases.Update
{
    public class UpdateMatch
    {
        private readonly MatchService _matchService;

        public UpdateMatch(MatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task<MatchResponseDTO> Execute(MatchRequestDTO matchDTO)
        {
            if (matchDTO == null)
                throw new ArgumentNullException(nameof(matchDTO), "Los detalles del partido no pueden ser nulos.");

            var existingMatch = await _matchService.GetMatchByIdAsync(new MatchID(matchDTO.MatchID));
            if (existingMatch == null)
                throw new InvalidOperationException("El partido no existe. No se puede actualizar.");

            var team1 = new Team(new TeamID(matchDTO.Team1ID), new TeamName("Equipo 1"), null, DateTime.UtcNow, "logo_url");
            var team2 = new Team(new TeamID(matchDTO.Team2ID), new TeamName("Equipo 2"), null, DateTime.UtcNow, "logo_url");

            existingMatch.Update(
                team1,
                team2,
                matchDTO.MatchDate,
                (MatchStatus)Enum.Parse(typeof(MatchStatus), matchDTO.Status),
                matchDTO.Location
            );

            await _matchService.UpdateMatchAsync(existingMatch);
            return new MatchResponseDTO
            {
                MatchID = existingMatch.MatchID,
                Team1 = existingMatch.Team1,
                Team2 = existingMatch.Team2,
                MatchDate = existingMatch.MatchDate,
                Status = existingMatch.Status.ToString(),
                Location = existingMatch.Location,
                CreatedAt = existingMatch.CreatedAt
            };
        }

    }
}
