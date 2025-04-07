using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Shared;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.Teams.Mappers
{
    public static class TeamMapper
    {
        // Mapea desde la entidad de base de datos a la entidad de dominio.
        // Se recibe el coach ya convertido a dominio (si corresponde) o null.
        public static Team MapToDomain(this TeamEntity entity, User? coach)
        {
            return new Team(
                new TeamID(entity.TeamID),
                new TeamName(entity.Name),
                coach,
                entity.CreatedAt,
                entity.Logo
            );
        }

        // Mapea desde la entidad de dominio a la entidad de base de datos.
        public static TeamEntity MapToEntity(this Team team)
        {
            return new TeamEntity
            {
                TeamID = team.TeamID.Value,
                Name = team.Name.Value,
                Logo = team.Logo,
                CoachID = team.Coach?.UserID.Value ?? 0, 
                CreatedAt = team.CreatedAt
            };
        }
    }
}
