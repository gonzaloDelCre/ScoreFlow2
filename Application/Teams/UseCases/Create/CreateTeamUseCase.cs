using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using System;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeamUseCase
    {
        private readonly ITeamRepository _teamRepository;

        public CreateTeamUseCase(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamResponseDTO> ExecuteAsync(TeamRequestDTO teamRequestDTO)
        {
            var team = new Team(
                new TeamID(0),
                new TeamName(teamRequestDTO.Name),
                DateTime.UtcNow,
                teamRequestDTO.Logo
            );

            team.SetCategory(teamRequestDTO.Category);
            team.SetClub(teamRequestDTO.Club);
            team.SetStadium(teamRequestDTO.Stadium);

            await _teamRepository.AddAsync(team);

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
