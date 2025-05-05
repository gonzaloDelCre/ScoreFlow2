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
        public static League MapToDomain(LeagueEntity entity)
        {
            return new League(
                new LeagueID(entity.LeagueID),
                new LeagueName(entity.Name),
                entity.Description
            );
        }

        // Método simple usado en repositorios para evitar carga excesiva
        public static League MapToDomainSimple(LeagueEntity entity)
        {
            return new League(
                new LeagueID(entity.LeagueID),
                new LeagueName(entity.Name),
                entity.Description
            );
        }

        public static LeagueEntity MapToEntity(League domain)
        {
            return new LeagueEntity
            {
                LeagueID = domain.LeagueID.Value,
                Name = domain.Name.Value,
                Description = domain.Description
            };
        }
    }
}
