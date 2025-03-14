using Application.Teams.DTOs;
using Domain.Ports.Teams;
using Domain.Services.Teams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class GetAllTeams
    {
        private readonly TeamService _teamService;

        public GetAllTeams(TeamService teamService)
        {
            _teamService = teamService;
        }

        // Ejecuta la obtención de todos los equipos
        public async Task<IEnumerable<TeamResponseDTO>> Execute()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            var result = new List<TeamResponseDTO>();

            foreach (var team in teams)
            {
                result.Add(new TeamResponseDTO
                {
                    TeamID = team.TeamID.Value,
                    TeamName = team.Name.Value,  // Accedemos al valor de TeamName
                    LogoUrl = team.Logo,         // Accedemos al logo
                    CreatedAt = team.CreatedAt
                });
            }

            return result;
        }
    }
}
