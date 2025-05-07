using Domain.Entities.Leagues;
using Domain.Shared;
using Infrastructure.Persistence.Leagues.Entities;

namespace Infrastructure.Persistence.Leagues.Mapper
{
    public class LeagueMapper : ILeagueMapper
    {
        public League MapToDomain(LeagueEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new League(
                new LeagueID(entity.ID),
                new LeagueName(entity.Name),
                entity.Description,
                entity.CreatedAt
            );
        }

        public LeagueEntity MapToEntity(League domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            return new LeagueEntity
            {
                ID = domain.LeagueID.Value,
                Name = domain.Name.Value,
                Description = domain.Description,
                CreatedAt = domain.CreatedAt
            };
        }
    }
}