using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Ports.Users;
using Domain.Services.Teams;
using Domain.Shared;

namespace Application.Teams.UseCases.Update
{
    public class UpdateTeamUseCase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public UpdateTeamUseCase(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<TeamResponseDTO> ExecuteAsync(TeamRequestDTO teamRequestDTO)
        {
            var teamID = new TeamID(teamRequestDTO.TeamID);  
            var team = await _teamRepository.GetByIdAsync(teamID);
            if (team == null)
                throw new Exception("El equipo no existe.");

            var coachID = new UserID(teamRequestDTO.CoachID);
            var coach = await _userRepository.GetByIdAsync(coachID);
            if (coach == null)
                throw new Exception("El entrenador no existe.");

            team.Update(
                new TeamName(teamRequestDTO.Name),
                coach,
                teamRequestDTO.Logo
            );

            await _teamRepository.UpdateAsync(team);

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
