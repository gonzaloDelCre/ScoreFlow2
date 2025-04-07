using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Ports.Users;
using Domain.Services.Teams;
using Domain.Shared;

namespace Application.Teams.UseCases.Create
{
    public class CreateTeamUseCase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public CreateTeamUseCase(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<TeamResponseDTO> ExecuteAsync(TeamRequestDTO teamRequestDTO)
        {
            var coachID = new UserID(teamRequestDTO.CoachID);
            var coach = await _userRepository.GetByIdAsync(coachID);
            if (coach == null)
                throw new Exception("El entrenador no existe.");

            var team = new Team(
                new TeamID(0),
                new TeamName(teamRequestDTO.Name),
                coach,
                DateTime.UtcNow,
                teamRequestDTO.Logo
            );

            await _teamRepository.AddAsync(team);

            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,
                CoachID = coach.UserID.Value,
                PlayerIds = team.Players.Select(p => p.PlayerID.Value).ToList(),
                LogoUrl = team.Logo,
                CreatedAt = team.CreatedAt
            };
        }
    }

}
