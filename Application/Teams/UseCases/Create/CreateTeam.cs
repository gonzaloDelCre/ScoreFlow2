using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Services.Teams;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeam
    {
        private readonly TeamService _teamService;

        public CreateTeam(TeamService teamService)
        {
            _teamService = teamService;
        }

        // Ejecuta la creación de un nuevo equipo
        public async Task<TeamResponseDTO> Execute(TeamRequestDTO teamDTO)
        {
            if (teamDTO == null)
                throw new ArgumentNullException(nameof(teamDTO), "Los detalles del equipo no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(teamDTO.Name))
                throw new ArgumentException("El nombre del equipo es obligatorio.");

            if (string.IsNullOrWhiteSpace(teamDTO.Logo))
                throw new ArgumentException("La URL del logo es obligatoria.");

            // Aquí creamos un nuevo objeto Team usando los datos del DTO
            var team = await _teamService.CreateTeamAsync(
                teamDTO.Name, // Creamos un nuevo TeamName
                teamDTO.CoachID,            // Usamos el CoachID
                teamDTO.Logo                // Logo
            );

            // Devolvemos un DTO de respuesta con los detalles del equipo recién creado
            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value, // Usamos el valor de TeamID
                TeamName = team.Name.Value, // Accedemos al valor de TeamName, ahora se convierte en string
                LogoUrl = team.Logo,        // Logo
                CreatedAt = team.CreatedAt  // Fecha de creación
            };
        }
    }
}
