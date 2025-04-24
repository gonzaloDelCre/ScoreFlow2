using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Teams.UseCases.Update
{
    public class UpdateTeamUseCase
    {
        private readonly ITeamRepository _teamRepository;

        public UpdateTeamUseCase(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamResponseDTO> ExecuteAsync(TeamRequestDTO teamRequestDTO)
        {
            var teamID = new TeamID(teamRequestDTO.TeamID);
            var team = await _teamRepository.GetByIdAsync(teamID);
            if (team == null)
                throw new Exception("El equipo no existe.");

            team.Update(
                new TeamName(teamRequestDTO.Name),
                teamRequestDTO.Logo,
                teamRequestDTO.Category,
                teamRequestDTO.Club,
                teamRequestDTO.Stadium
            );

            await _teamRepository.UpdateAsync(team);

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
