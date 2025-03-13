using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Enum;
using Infrastructure.Persistence.Players.Entities;

namespace Infrastructure.Persistence.Players.Mapper
{
    public class PlayerMapper
    {
        public PlayerEntity MapToEntity(Player player)
        {
            return new PlayerEntity
            {
                PlayerID = player.PlayerID,
                UserID = player.User?.UserID.Value,
                TeamID = player.Team.TeamID,
                Position = player.Position.ToString(),
                Dorsal = player.Dorsal,
                CreatedAt = player.CreatedAt
            };
        }

        public Player MapToDomain(PlayerEntity entity, Team team, User? user)
        {
            return new Player(
                entity.PlayerID,
                user,
                team,
                Enum.Parse<PlayerPosition>(entity.Position),
                entity.Dorsal,
                entity.CreatedAt
            );
        }
    }
}
