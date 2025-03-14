using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Services.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Update
{
    public class UpdateTeam
    {
        private readonly TeamService _teamService;

        public UpdateTeam(TeamService teamService)
        {
            _teamService = teamService;
        }

        // Ejecuta la actualización de un equipo existente
        public async Task<Team> Execute(TeamRequestDTO teamDTO)
        {
            if (teamDTO == null)
                throw new ArgumentNullException(nameof(teamDTO), "Los detalles del equipo no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(teamDTO.Name))  // Usamos Name en lugar de TeamName
                throw new ArgumentException("El nombre del equipo es obligatorio.");

            if (string.IsNullOrWhiteSpace(teamDTO.Logo))  // Usamos Logo en lugar de LogoUrl
                throw new ArgumentException("La URL del logo es obligatoria.");

            var existingTeam = await _teamService.GetTeamByIdAsync(new TeamID(teamDTO.CoachID));  // Asumí que coachID es el ID del equipo
            if (existingTeam == null)
                throw new InvalidOperationException("El equipo no existe. No se puede actualizar.");

            // Actualiza el equipo con los nuevos detalles
            existingTeam.Update(new TeamName(teamDTO.Name), teamDTO.Logo);  // Usamos el Name para crear TeamName y Logo directamente

            await _teamService.UpdateTeamAsync(existingTeam);
            return existingTeam;
        }
    }
}
