using Application.Teams.DTOs;
using Domain.Ports.Teams;
using Domain.Services.Teams;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class GetTeamById
    {
        private readonly TeamService _teamService;

        public GetTeamById(TeamService teamService)
        {
            _teamService = teamService;
        }

        // Ejecuta la obtención de un equipo por su ID
        public async Task<TeamResponseDTO?> Execute(TeamID teamId)
        {
            // Verificamos si el ID es válido (por ejemplo, si el TeamID tiene un valor)
            if (teamId == null)
                throw new ArgumentException("El ID del equipo no puede ser nulo");

            var team = await _teamService.GetTeamByIdAsync(teamId);
            if (team == null)
                return null;

            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,  // Accedemos al valor de TeamName
                LogoUrl = team.Logo,         // Accedemos al logo
                CreatedAt = team.CreatedAt
            };
        }
    }
}
