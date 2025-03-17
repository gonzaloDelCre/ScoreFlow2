using Application.Leagues.DTOs;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Update
{
    public class UpdateLeague
    {
        private readonly LeagueService _leagueService;

        public UpdateLeague(LeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        public async Task<LeagueResponseDTO> Execute(LeagueRequestDTO leagueDTO, int leagueId)
        {
            if (leagueDTO == null)
                throw new ArgumentNullException(nameof(leagueDTO), "La liga no puede ser nula.");

            var existingLeague = await _leagueService.GetLeagueByIdAsync(new LeagueID(leagueId));
            if (existingLeague == null)
                throw new InvalidOperationException("La liga no existe. No se puede actualizar.");

            existingLeague.Update(
                new LeagueName(leagueDTO.Name),
                leagueDTO.Description,
                leagueDTO.CreatedAt
            );

            await _leagueService.UpdateLeagueAsync(existingLeague);
            return new LeagueResponseDTO
            {
                LeagueID = existingLeague.LeagueID.Value,
                Name = existingLeague.Name.Value,
                Description = existingLeague.Description,
                CreatedAt = existingLeague.CreatedAt
            };
        }
    }
}
