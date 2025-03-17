using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;

namespace Application.Teams.Mapper
{
    public class TeamMapper
    {
        public TeamResponseDTO MapToDTO(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "La entidad de dominio Team no puede ser nula.");

            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,
                LogoUrl = team.Logo,
                CoachID = team.Coach?.UserID.Value ?? 0,
                CoachName = team.Coach?.FullName.Value ?? string.Empty,
                CreatedAt = team.CreatedAt
            };
        }

        public Team MapToDomain(TeamRequestDTO teamDTO, User? coach = null, Team? existingTeam = null)
        {
            if (teamDTO == null)
                throw new ArgumentNullException(nameof(teamDTO), "El DTO TeamRequestDTO no puede ser nulo.");

            if (existingTeam != null)
            {
                return new Team(
                    existingTeam.TeamID,
                    new TeamName(teamDTO.Name),
                    coach,
                    existingTeam.CreatedAt,
                    teamDTO.Logo
                );
            }

            return new Team(
                new TeamID(0),
                new TeamName(teamDTO.Name),
                coach,
                DateTime.UtcNow,
                teamDTO.Logo
            );
        }
    }
}
