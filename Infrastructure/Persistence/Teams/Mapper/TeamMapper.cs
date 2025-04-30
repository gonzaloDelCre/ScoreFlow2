using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Leagues;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Shared;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.Teams.Mapper
{
    public static class TeamMapper
    {
        /// <summary>
        /// Mappea un Team de dominio a TeamEntity (persistencia),
        /// incluyendo la FK LeagueID.
        /// </summary>
        public static TeamEntity MapToEntity(this Team domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new TeamEntity
            {
                TeamID = domain.TeamID.Value,
                Name = domain.Name.Value,
                Logo = domain.Logo,
                Category = domain.Category,
                Club = domain.Club,
                Stadium = domain.Stadium,
                ExternalID = domain.ExternalID,
                CreatedAt = domain.CreatedAt,
                CoachPlayerID = domain.Coach?.PlayerID.Value,

                // ← aquí asignas la FK de liga
                LeagueID = domain.LeagueID.Value
            };
        }

        /// <summary>
        /// Mappea un TeamEntity (persistencia) a Team (dominio),
        /// reconociendo la liga a la que pertenece y los jugadores.
        /// </summary>
        public static Team MapToDomain(
            this TeamEntity entity,
            League league,
            ICollection<TeamPlayerEntity> teamPlayers,
            ICollection<PlayerEntity> playerEntities)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (league == null) throw new ArgumentNullException(nameof(league));

            // 1) Construyo la lista de Player de dominio
            var players = playerEntities
                .Where(p => teamPlayers.Any(tp =>
                    tp.TeamID == entity.TeamID &&
                    tp.PlayerID == p.PlayerID))
                .Select(p => new Player(
                    new PlayerID(p.PlayerID),
                    new PlayerName(p.Name),
                    Enum.TryParse<Domain.Enum.PlayerPosition>(p.Position, out var pos)
                        ? pos
                        : Domain.Enum.PlayerPosition.JUGADOR,
                    new Domain.Entities.Players.PlayerAge(p.Age),
                    p.Goals,
                    p.Photo,
                    p.CreatedAt,
                    null
                ))
                .ToList();

            // 2) Creo la instancia Team con la liga
            var team = new Team(
                new TeamID(entity.TeamID),
                new TeamName(entity.Name),
                league,
                entity.Logo ?? string.Empty,
                entity.CreatedAt,
                entity.ExternalID
            );

            // 3) Resto de propiedades
            team.Update(entity.Category, entity.Club, entity.Stadium);

            foreach (var pl in players)
                team.AddPlayer(pl);

            // coach
            if (entity.CoachPlayerID.HasValue)
            {
                var coachData = playerEntities
                    .FirstOrDefault(p => p.PlayerID == entity.CoachPlayerID.Value);
                if (coachData != null)
                {
                    var coach = new Player(
                        new PlayerID(coachData.PlayerID),
                        new PlayerName(coachData.Name),
                        Enum.TryParse<Domain.Enum.PlayerPosition>(coachData.Position, out var cp)
                            ? cp
                            : Domain.Enum.PlayerPosition.JUGADOR,
                        new Domain.Entities.Players.PlayerAge(coachData.Age),
                        coachData.Goals,
                        coachData.Photo,
                        coachData.CreatedAt,
                        null
                    );
                    team.AssignCoach(coach);
                }
            }

            return team;
        }
    }
}
