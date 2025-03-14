using Application.Matches.DTOs;
using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Ports.Matches;
using Domain.Services.Matches;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Update
{
    public class UpdateMatch
    {
        private readonly MatchService _matchService;

        public UpdateMatch(MatchService matchService)
        {
            _matchService = matchService;
        }

        // Ejecuta la actualización de un partido existente
        public async Task<Match> Execute(MatchRequestDTO matchDTO)
        {
            if (matchDTO == null)
                throw new ArgumentNullException(nameof(matchDTO), "Los detalles del partido no pueden ser nulos.");

            var existingMatch = await _matchService.GetMatchByIdAsync(new MatchID(matchDTO.MatchID));
            if (existingMatch == null)
                throw new InvalidOperationException("El partido no existe. No se puede actualizar.");

            var status = (MatchStatus)Enum.Parse(typeof(MatchStatus), matchDTO.Status);

            var team1 = new Team(
                new TeamID(matchDTO.Team1ID), 
                new TeamName("Nombre del Equipo 1"), 
                null, 
                DateTime.UtcNow, 
                "logo_url" 
            );

            var team2 = new Team(
                new TeamID(matchDTO.Team2ID),
                new TeamName("Nombre del Equipo 2"),
                null,
                DateTime.UtcNow,
                "logo_url"
            );

            // Actualiza el partido
            existingMatch.Update(
                team1,
                team2,
                matchDTO.MatchDate,
                status,
                matchDTO.Location
            );

            await _matchService.UpdateMatchAsync(existingMatch);
            return existingMatch;
        }
    }
}
