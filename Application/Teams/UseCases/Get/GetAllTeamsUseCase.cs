using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Services.Teams;

namespace Application.Teams.UseCases.Get
{
    public class GetAllTeamsUseCase
    {
        private readonly ITeamRepository _teamRepository;

        public GetAllTeamsUseCase(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<List<TeamResponseDTO>> ExecuteAsync()
        {
            var teams = await _teamRepository.GetAllAsync();

            return teams.Select(team => new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                Name = team.Name.Value,
                CoachID = team.Coach.UserID.Value,
                PlayerIds = team.Players.Select(p => p.PlayerID.Value).ToList(),
                Logo = team.Logo,
                CreatedAt = team.CreatedAt
            }).ToList();
        }
    }

}
