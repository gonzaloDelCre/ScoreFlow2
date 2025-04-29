using Domain.Entities.TeamPlayers;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Enum;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Domain.Shared;

namespace Infrastructure.Persistence.TeamPlayers.Mappers
{
    public static class TeamPlayerMapper
    {
        public static TeamPlayerEntity MapToEntity(TeamPlayer teamPlayer)
        {
            return new TeamPlayerEntity
            {
                TeamID = teamPlayer.TeamID.Value,
                PlayerID = teamPlayer.PlayerID.Value,
                JoinedAt = teamPlayer.JoinedAt,
                RoleInTeam = teamPlayer.RoleInTeam.HasValue ? teamPlayer.RoleInTeam.Value : default(RoleInTeam)
            };
        }

        public static TeamPlayer MapEntityToDomain(TeamPlayerEntity e)
        {
            // Build domain Team
            var t = e.Team;
            var team = new Team(
                new TeamID(t.TeamID),
                new TeamName(t.Name),
                t.CreatedAt,
                t.Logo,
                t.ExternalID
            );

            // Build domain Player
            var p = e.Player;
            // parse the stored enum string back into PlayerPosition if needed, otherwise default:
            var position = Enum.TryParse<PlayerPosition>(p.Position, true, out var pos) ? pos : PlayerPosition.JUGADOR;

            var player = new Player(
                new PlayerID(p.PlayerID),
                new PlayerName(p.Name),
                position,
                new PlayerAge(p.Age),
                p.Goals,
                p.Photo,
                p.CreatedAt,
                new List<TeamPlayer>()  // avoid circular deps
            );

            return new TeamPlayer(
                new TeamID(e.TeamID),
                new PlayerID(e.PlayerID),
                e.JoinedAt,
                e.RoleInTeam,
                team,
                player
            );
        }
    }
}
