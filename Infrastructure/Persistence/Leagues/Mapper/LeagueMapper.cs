using Domain.Entities.Leagues;
using Domain.Shared;
using Infrastructure.Persistence.Leagues.Entities;

namespace Infrastructure.Persistence.Leagues.Mapper
{
    public class LeagueMapper
    {
        public LeagueEntity MapToEntity(League league)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league), "La entidad de dominio League no puede ser nula.");

            return new LeagueEntity
            {
                LeagueID = league.LeagueID.Value,
                Name = league.Name.Value,
                Description = league.Description,
                CreatedAt = league.CreatedAt
            };
        }

        public League MapToDomain(LeagueEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad persistente LeagueEntity no puede ser nula.");

            return new League(
                new LeagueID(entity.LeagueID),
                new LeagueName(entity.Name),
                entity.Description,
                entity.CreatedAt
            );
        }
    }
}