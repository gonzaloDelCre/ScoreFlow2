using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Update
{
    public class UpdateLeague
    {
        private readonly ILeagueRepository _leagueRepository;

        public UpdateLeague(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        // Ejecuta la actualización de una liga existente
        public async Task<League> Execute(LeagueRequestDTO leagueDTO)
        {
            var existingLeague = await _leagueRepository.GetByNameAsync(leagueDTO.Name);
            if (existingLeague == null)
            {
                throw new InvalidOperationException("La liga no existe. No se puede actualizar.");
            }

            // Si la liga existe, actualizarla
            existingLeague.Update(
                new LeagueName(leagueDTO.Name),
                leagueDTO.Description,
                leagueDTO.CreatedAt
            );

            await _leagueRepository.UpdateAsync(existingLeague);
            return existingLeague;
        }
    }

}
