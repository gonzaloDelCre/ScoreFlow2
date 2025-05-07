using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Leagues;
using Domain.Shared;
using Infrastructure.Persistence.Leagues.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Teams.Mapper;

namespace Infrastructure.Persistence.Leagues.Mapper
{
    public static class LeagueMapper
    {
        public static LeagueEntity MapToEntity(this League domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new LeagueEntity
            {
                Name = domain.Name.Value,
                Description = domain.Description,
                CreatedAt = domain.CreatedAt
            };
        }

        public static League MapToDomain(this LeagueEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new League(
                new LeagueID(entity.LeagueID),
                new LeagueName(entity.Name),
                entity.Description,
                entity.CreatedAt
            );
        }

        public static League MapToDomainSimple(this LeagueEntity entity)
        {
            return new League(
                new LeagueID(entity.LeagueID),
                new LeagueName(entity.Name),
                entity.Description,
                entity.CreatedAt
            );
        }
    }
}
