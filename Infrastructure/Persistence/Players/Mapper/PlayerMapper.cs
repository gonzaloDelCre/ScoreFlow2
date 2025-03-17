using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Infrastructure.Persistence.Players.Entities;

namespace Infrastructure.Persistence.Players.Mapper
{
    public class PlayerMapper
    {
        public PlayerEntity MapToEntity(Player player)
        {
            return new PlayerEntity
            {
                PlayerID = player.PlayerID.Value,
                TeamID = player.TeamID.Value,
                Position = player.Position.ToString(),
                CreatedAt = player.CreatedAt,
                Name = player.Name.Value 
            };
        }

        public Player MapToDomain(PlayerEntity entity, Team team)
        {
            return new Player(
                new PlayerID(entity.PlayerID),
                new PlayerName(entity.Name), 
                new TeamID(entity.TeamID),
                Enum.Parse<PlayerPosition>(entity.Position),
                team,
                entity.CreatedAt
            );
        }
    }
}
