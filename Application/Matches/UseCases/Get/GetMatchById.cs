using Application.Matches.DTOs;
using Application.Matches.Mapper;
using Domain.Ports.Matches;
using Domain.Services.Matches;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Get
{
    public class GetMatchById
    {
        private readonly MatchService _matchService;

        public GetMatchById(MatchService matchService)
        {
            _matchService = matchService;
        }

        // Ejecuta la obtención de un partido por su ID
        public async Task<MatchResponseDTO?> Execute(MatchID matchID)
        {
            if (matchID == null)
                throw new ArgumentNullException(nameof(matchID), "El ID del partido no puede ser nulo.");

            var match = await _matchService.GetMatchByIdAsync(matchID);
            if (match == null)
                return null;

            return new MatchResponseDTO
            {
                MatchID = match.MatchID,
                Team1 = match.Team1,
                Team2 = match.Team2,
                MatchDate = match.MatchDate,
                Status = match.Status.ToString(), // Convierte el enum a string si es necesario
                Location = match.Location,
                CreatedAt = match.CreatedAt
            };
        }
    }
}
