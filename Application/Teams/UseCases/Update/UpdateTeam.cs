using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Services.Teams;
using Domain.Shared;
using System;
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

        public async Task<TeamResponseDTO> Execute(TeamRequestDTO teamDTO, int teamID) 
        {
            if (teamDTO == null)
                throw new ArgumentNullException(nameof(teamDTO), "Los detalles del equipo no pueden ser nulos.");

            var existingTeam = await _teamService.GetTeamByIdAsync(new TeamID(teamID)); 
            if (existingTeam == null)
                throw new InvalidOperationException("El equipo no existe. No se puede actualizar.");

            existingTeam.Update(new TeamName(teamDTO.Name), teamDTO.Logo);

            await _teamService.UpdateTeamAsync(existingTeam);
            return new TeamResponseDTO
            {
                TeamID = existingTeam.TeamID.Value,
                TeamName = existingTeam.Name.Value,
                LogoUrl = existingTeam.Logo,
                CreatedAt = existingTeam.CreatedAt
            };
        }

    }
}
