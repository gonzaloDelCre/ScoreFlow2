using System.Linq;
using Domain.Entities.Teams;
using Domain.Entities.Players;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Domain.Shared;
using Infrastructure.Persistence.Players.Entities;

namespace Infrastructure.Persistence.Teams.Mapper
{
    public static class TeamMapper
    {
        public static Team MapToDomain(
            TeamEntity entity,
            ICollection<TeamPlayerEntity> teamPlayers,
            ICollection<PlayerEntity> playerEntities)
        {
            // Mapeo de jugadores asociados al equipo
            var players = playerEntities
                .Where(p => teamPlayers.Any(tp => tp.TeamID == entity.TeamID && tp.PlayerID == p.PlayerID))
                .Select(p => new Player(
                    new PlayerID(p.PlayerID),
                    new PlayerName(p.Name),
                    Enum.Parse<Domain.Enum.PlayerPosition>(p.Position),
                    new Domain.Entities.Players.PlayerAge(p.Age),
                    p.Goals,
                    p.Photo,
                    p.CreatedAt,
                    null // No asignamos directamente el equipo aquí
                ))
                .ToList();

            // Crear el equipo base
            var team = new Team(
                new TeamID(entity.TeamID),
                new TeamName(entity.Name),
                entity.CreatedAt,
                entity.Logo,
                entity.ExternalID
            );

            team.Update(
                category: entity.Category,
                club: entity.Club,
                stadium: entity.Stadium
            );

            // Agregar jugadores
            foreach (var player in players)
            {
                team.AddPlayer(player);
            }

            // Asignar el entrenador si existe
            if (entity.CoachPlayerID.HasValue)
            {
                var coachEntity = playerEntities.FirstOrDefault(p => p.PlayerID == entity.CoachPlayerID.Value);
                if (coachEntity != null)
                {
                    var coach = new Player(
                        new PlayerID(coachEntity.PlayerID),
                        new PlayerName(coachEntity.Name),
                        Enum.Parse<Domain.Enum.PlayerPosition>(coachEntity.Position),
                        new Domain.Entities.Players.PlayerAge(coachEntity.Age),
                        coachEntity.Goals,
                        coachEntity.Photo,
                        coachEntity.CreatedAt,
                        null
                    );
                    team.AssignCoach(coach);
                }
            }

            return team;
        }

        public static TeamEntity MapToEntity(Team team)
        {
            return new TeamEntity
            {
                TeamID = team.TeamID.Value,
                Name = team.Name.Value,
                Logo = team.Logo,
                CreatedAt = team.CreatedAt,
                ExternalID = team.ExternalID,
                Category = team.Category,
                Club = team.Club,
                Stadium = team.Stadium,
                CoachPlayerID = team.Coach?.PlayerID.Value
            };
        }
    }
}
