using Application.Matches.DTOs;
using Domain.Entities.Matches;
using Domain.Enum;
using Domain.Ports.Teams;
using Domain.Shared;
using System;

namespace Application.Matches.Mapper
{
    public class MatchMapper
    {
        public MatchResponseDTO MapToDTO(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match), "La entidad de dominio Match no puede ser nula.");

            return new MatchResponseDTO
            {
                MatchID = match.MatchID,
                Team1 = match.Team1,
                Team2 = match.Team2,
                MatchDate = match.MatchDate,
                Status = match.Status.ToString(),
                Location = match.Location,
                CreatedAt = match.CreatedAt
            };
        }

        public async Task<Match> MapToDomainAsync(MatchRequestDTO matchDTO, ITeamRepository teamRepository)
        {
            if (matchDTO == null)
                throw new ArgumentNullException(nameof(matchDTO), "El DTO MatchRequestDTO no puede ser nulo.");

            var team1 = await teamRepository.GetByIdAsync(new TeamID(matchDTO.Team1ID));
            var team2 = await teamRepository.GetByIdAsync(new TeamID(matchDTO.Team2ID));

            if (team1 == null || team2 == null)
                throw new InvalidOperationException("Uno o ambos equipos no existen.");

            MatchStatus status;
            try
            {
                status = Enum.Parse<MatchStatus>(matchDTO.Status);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"El valor de status '{matchDTO.Status}' no es válido.");
            }

            return new Match(
                new MatchID(matchDTO.MatchID),
                team1,
                team2,
                matchDTO.MatchDate,
                status,
                matchDTO.Location ?? string.Empty
            );
        }
    }
}
