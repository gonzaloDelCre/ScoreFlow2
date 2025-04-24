using Application.Teams.DTOs;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Get
{
    public class GetTeamByIdUseCase
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamByIdUseCase(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamResponseDTO> ExecuteAsync(int teamID)
        {
            var teamId = new TeamID(teamID);

            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new Exception("El equipo no existe.");

            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,
                PlayerIds = team.Players.Select(p => p.PlayerID.Value).ToList(),
                LogoUrl = team.Logo,
                CreatedAt = team.CreatedAt,
                Category = team.Category,
                Club = team.Club,
                Stadium = team.Stadium
            };
        }
    }
}
