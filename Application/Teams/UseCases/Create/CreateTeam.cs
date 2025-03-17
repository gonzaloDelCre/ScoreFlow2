using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Services.Teams;
using Domain.Shared;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeam
    {
        private readonly ITeamRepository _teamRepository;
        private readonly TeamService _teamService;

        public CreateTeam(ITeamRepository teamRepository, TeamService teamService)
        {
            _teamRepository = teamRepository;
            _teamService = teamService;
        }

        public async Task<TeamResponseDTO> Execute(TeamRequestDTO teamDTO)
        {
            if (teamDTO == null)
                throw new ArgumentNullException(nameof(teamDTO), "Los detalles del equipo no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(teamDTO.Name))
                throw new ArgumentException("El nombre del equipo es obligatorio.");


            var team = await _teamService.CreateTeamAsync(
                teamDTO.Name, teamDTO.CoachID, teamDTO.Logo
            );

            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,
                LogoUrl = team.Logo,
                CreatedAt = team.CreatedAt
            };
        }
    }
}
