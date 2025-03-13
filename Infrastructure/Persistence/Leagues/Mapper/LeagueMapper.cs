using Domain.Entities.Leagues;
using Infrastructure.Persistence.Leagues.Entities;

namespace Infrastructure.Persistence.Leagues.Mapper
{
    public class LeagueMapper
    {
        public LeagueEntity MapToEntity(League league)
        {
            return new LeagueEntity
            {
                LeagueID = league.LeagueID,
                Name = league.Name,
                Description = league.Description,
                CreatedAt = league.CreatedAt
            };
        }

        public League MapToDomain(LeagueEntity entity)
        {
            return new League(
                entity.LeagueID,
                entity.Name,
                entity.Description,
                entity.CreatedAt
            );
        }
    }
}
