using Application.Teams.DTOs;
using Domain.Ports.Teams;
using System.Linq;
using System.Threading.Tasks;

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
                TeamName = team.Name.Value,
                PlayerIds = team.Players.Select(p => p.PlayerID.Value).ToList(),
                LogoUrl = team.Logo,
                CreatedAt = team.CreatedAt
            }).ToList();
        }
    }
}
