using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.Players.Mapper
{
    public class PlayerMapper
    {
        public static PlayerEntity MapToEntity(Player player)
        {
            return new PlayerEntity
            {
                PlayerID = player.PlayerID.Value,
                Name = player.Name.Value,
                Position = player.Position.ToString(), // Convertimos a string
                Age = player.Age.Value,
                Goals = player.Goals,
                Photo = player.Photo,
                CreatedAt = player.CreatedAt
            };
        }

        public static Player MapToDomain(PlayerEntity entity, ICollection<TeamPlayerEntity> teamPlayers)
        {
            var teamPlayerList = teamPlayers
                .Where(tp => tp.PlayerID == entity.PlayerID)
                .Select(tp => new TeamPlayer(
                    new TeamID(tp.TeamID),
                    new PlayerID(entity.PlayerID),
                    tp.JoinedAt,
                    tp.RoleInTeam
                )).ToList();

            return new Player(
                new PlayerID(entity.PlayerID),
                new PlayerName(entity.Name),
                Enum.Parse<PlayerPosition>(entity.Position),
                new PlayerAge(entity.Age),
                entity.Goals,
                entity.Photo,
                entity.CreatedAt,
                teamPlayerList
            );
        }

    }
}
