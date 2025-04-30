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
    public class LeagueMapper
    {
        private readonly TeamMapper _teamMapper;

        public LeagueMapper()
        {
            _teamMapper = new TeamMapper();
        }

        /// <summary>
        /// Mapea una League de dominio a LeagueEntity, incluyendo equipos.
        /// </summary>
        public LeagueEntity MapToEntity(League league)
        {
            if (league == null) throw new ArgumentNullException(nameof(league));

            return new LeagueEntity
            {
                LeagueID = league.LeagueID.Value,
                Name = league.Name.Value,
                Description = league.Description,
                CreatedAt = league.CreatedAt,
                Teams = league.Teams
                                  .Select(team =>
                                  {
                                      var entity = _teamMapper.MapToEntity(team);
                                      entity.LeagueID = league.LeagueID.Value;
                                      return entity;
                                  })
                                  .ToList()
            };
        }

        /// <summary>
        /// Mapea una LeagueEntity a League de dominio. 
        /// Requiere pasar todas las relaciones de TeamPlayers y PlayerEntities
        /// para que TeamMapper pueda construir correctamente los equipos.
        /// </summary>
        public League MapToDomain(
            LeagueEntity entity,
            IEnumerable<TeamPlayerEntity> allTeamPlayers,
            IEnumerable<PlayerEntity> allPlayers)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // 1) Creamos la instancia de League
            var league = new League(
                new LeagueID(entity.LeagueID),
                new LeagueName(entity.Name),
                entity.Description,
                entity.CreatedAt
            );

            // 2) Agrupamos TeamPlayerEntity por TeamID para acelerar búsquedas
            var teamPlayersLookup = allTeamPlayers
                .GroupBy(tp => tp.TeamID)
                .ToDictionary(g => g.Key, g => (ICollection<TeamPlayerEntity>)g.ToList());

            // 3) Transformamos cada TeamEntity en Team dominio
            foreach (var teamEntity in entity.Teams ?? Enumerable.Empty<Infrastructure.Persistence.Teams.Entities.TeamEntity>())
            {
                // obtenemos sólo los registros de TeamPlayer para este equipo
                teamPlayersLookup.TryGetValue(teamEntity.TeamID, out var teamPlayersForThisTeam);

                // invocamos la sobrecarga que necesita la liga, sus TeamPlayers y todos los PlayerEntities
                var team = _teamMapper.MapToDomain(
                    teamEntity,
                    league,
                    teamPlayersForThisTeam ?? Array.Empty<TeamPlayerEntity>(),
                    allPlayers.ToList()
                );

                league.AddTeam(team);
            }

            return league;
        }
    }
}
