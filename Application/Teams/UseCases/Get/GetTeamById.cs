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

        public async Task<TeamResponseDTO?> ExecuteAsync(TeamID teamId)
        {
            if (teamId == null)
                throw new ArgumentException("El ID del equipo no puede ser nulo.");

            var team = await _teamService.GetTeamByIdAsync(teamId);
            return team != null ? new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,
                LogoUrl = team.Logo,
                CreatedAt = team.CreatedAt
            } : null;
        }
    }
}
