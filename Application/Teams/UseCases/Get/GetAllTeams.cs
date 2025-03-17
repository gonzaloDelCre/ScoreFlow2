using Application.Teams.DTOs;
using Domain.Ports.Teams;
using Domain.Services.Teams;
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

        public async Task<IEnumerable<TeamResponseDTO>> ExecuteAsync()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return teams.Select(team => new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,
                LogoUrl = team.Logo,
                CreatedAt = team.CreatedAt
            });
        }
    }
}
