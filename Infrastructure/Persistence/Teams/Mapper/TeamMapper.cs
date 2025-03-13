using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Enum;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.Users.Entities;
using Domain.Shared;

namespace Infrastructure.Persistence.Teams.Mapper
{
    public class TeamMapper
    {
        public TeamEntity MapToEntity(Team team)
        {
            return new TeamEntity
            {
                TeamID = team.TeamID,
                Name = team.Name,
                Logo = team.Logo,
                CoachID = team.Coach.UserID.Value,
                CreatedAt = team.CreatedAt
            };
        }

        public Team MapToDomain(TeamEntity entity, User coach)
        {
            return new Team(
                entity.TeamID,
                entity.Name,
                entity.Logo,
                coach,
                entity.CreatedAt
            );
        }
    }
}
