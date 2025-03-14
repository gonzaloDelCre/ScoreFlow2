using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Enum;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.Users.Entities;
using Domain.Shared;

namespace Infrastructure.Persistence.Teams.Mapper
{
    public static class TeamMapper
    {
        public static Team ToDomain(TeamEntity entity, UserEntity? coachEntity = null)
        {
            if (entity == null)
            {
                return null;
            }

            var teamName = new TeamName(entity.Name);
            User coach = null;

            if (coachEntity != null)
            {
                coach = new User(
                    new UserID(coachEntity.UserID),
                    new UserFullName(coachEntity.FullName),
                    new UserEmail(coachEntity.Email),
                    new UserPasswordHash(coachEntity.PasswordHash),
                    Enum.Parse<UserRole>(coachEntity.Role),
                    coachEntity.CreatedAt
                );
            }

            return new Team(
                new TeamID(entity.TeamID),
                teamName,
                coach,
                entity.CreatedAt,
                entity.Logo
            );
        }


        public static TeamEntity ToEntity(Team domain)
        {
            if (domain == null)
            {
                return null;
            }

            return new TeamEntity
            {
                TeamID = domain.TeamID.Value,        // Convertimos TeamID a int
                Name = domain.Name.Value,            // Convertimos TeamName a string
                CoachID = domain.Coach?.UserID.Value ?? 0,  // Convertimos el CoachID si existe
                CreatedAt = domain.CreatedAt,        // Fecha de creación
                Logo = domain.Logo                   // Logo
            };
        }
    }
}
