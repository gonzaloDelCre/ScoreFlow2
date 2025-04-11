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

        public static TeamPlayer MapToDomain(TeamPlayerEntity entity, Team team, Player player)
        {
            return new TeamPlayer(
                new TeamID(entity.TeamID),
                new PlayerID(entity.PlayerID),
                entity.JoinedAt,
                entity.RoleInTeam,
                team,
                player
            );
        }
    }
}
